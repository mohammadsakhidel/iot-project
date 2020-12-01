using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TrackAPI.Constants;
using TrackAPI.Helpers;
using TrackAPI.Models;
using TrackDataAccess.Models.Identity;

namespace TrackAPI.Services {
    public class UserService : IUserService {

        private UserManager<AppUser> _userManager;
        private readonly AppSettings _appSettings;
        public UserService(UserManager<AppUser> userManager, IOptions<AppSettings> appSettings) {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<(bool, string)> CreateAsync(UserModel model) {

            // Check if password is empty or not cause the RequiredAttribute 
            // is removed to use single model for both creating and updating
            if (string.IsNullOrEmpty(model.Password))
                return (false, "Password is empty.");

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = new AppUser {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = model.IsActive,
                CreationTime = DateTime.UtcNow
            };

            var claims = new List<Claim> {
                    new Claim(ClaimNames.GIVEN_NAME, model.GivenName),
                    new Claim(ClaimNames.SURNAME, model.Surname),
                    new Claim(ClaimNames.EMAIL, model.Email),
                    new Claim(ClaimNames.STATE, model.State),
                    new Claim(ClaimNames.CITY, model.City),
                    new Claim(ClaimNames.ADDRESS, model.Address),
                    new Claim(ClaimNames.GROUP, UserGroups.USERS)
                };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            result = await _userManager.AddClaimsAsync(user, claims);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            transaction.Complete();
            return (true, user.Id);

        }

        public async Task<UserModel> FindByUsernameAsync(string userName) {
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser != null) {
                var claims = await _userManager.GetClaimsAsync(appUser);
                return mapEntityToModel(appUser, claims);
            } else {
                return null;
            }
        }

        public async Task<UserModel> GetAsync(string id) {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null) {
                var claims = await _userManager.GetClaimsAsync(appUser);
                return mapEntityToModel(appUser, claims);
            } else {
                return null;
            }
        }

        public async Task<List<UserModel>> GetAsync(int skip, int take) {
            var appUsers = await Task.Run(() => {
                return _userManager.Users.OrderByDescending(u => u.CreationTime).Skip(skip).Take(take).ToList();
            });

            var users = await Task.Run(() => {
                var list = new List<UserModel>();
                foreach (var user in appUsers) {
                    var claims = _userManager.GetClaimsAsync(user).Result;
                    list.Add(mapEntityToModel(user, claims));
                }
                return list;
            });

            return users;
        }

        public async Task<(bool, string)> RemoveAsync(string id) {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, "User not found.");

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            return (true, string.Empty);
        }

        public async Task<(bool, string)> UpdateAsync(UserModel model) {

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return (false, "User not found");

            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // Update Claims and Password:
            await Task.Run(() => {
                var claims = _userManager.GetClaimsAsync(user).Result;
                _userManager.ReplaceClaimAsync(user, claims.Single(c => c.Type == ClaimNames.GIVEN_NAME),
                    new Claim(ClaimNames.GIVEN_NAME, model.GivenName)).Wait();
                _userManager.ReplaceClaimAsync(user, claims.Single(c => c.Type == ClaimNames.SURNAME),
                    new Claim(ClaimNames.SURNAME, model.Surname)).Wait();
                _userManager.ReplaceClaimAsync(user, claims.Single(c => c.Type == ClaimNames.STATE),
                    new Claim(ClaimNames.STATE, model.State)).Wait();
                _userManager.ReplaceClaimAsync(user, claims.Single(c => c.Type == ClaimNames.CITY),
                    new Claim(ClaimNames.CITY, model.City)).Wait();
                _userManager.ReplaceClaimAsync(user, claims.Single(c => c.Type == ClaimNames.ADDRESS),
                    new Claim(ClaimNames.ADDRESS, model.Address)).Wait();

                // Change Password:
                if (!string.IsNullOrEmpty(model.Password)) {
                    _userManager.RemovePasswordAsync(user).Wait();
                    _userManager.AddPasswordAsync(user, model.Password).Wait();
                }
            });

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            transaction.Complete();
            return (true, string.Empty);

        }

        public async Task<(bool, string)> DeactivateAsync(string id) {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, "User not found.");

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            return (true, string.Empty);
        }

        public async Task<(bool, string)> ActivateAsync(string id) {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, "User not found.");

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            return (true, string.Empty);
        }

        public async Task<(bool isValid, string token)> ValidateUserAsync(string userName, string password) {
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser != null) {

                // Check Passord:
                var isPassValid = await _userManager.CheckPasswordAsync(appUser, password);
                if (!isPassValid)
                    return (false, "Username or password is not valid.");

                // Check Being Active:
                if (!appUser.IsActive)
                    return (false, "User is not active.");

                var token = await generateTokenAsync(appUser);
                return (true, token);

            } else {
                return (false, null);
            }
        }

        #region Private Methods:
        private static UserModel mapEntityToModel(AppUser appUser, IEnumerable<Claim> claims) {
            return new UserModel {
                Id = appUser.Id,
                GivenName = claims.SingleOrDefault(c => c.Type == ClaimNames.GIVEN_NAME)?.Value,
                Surname = claims.SingleOrDefault(c => c.Type == ClaimNames.SURNAME)?.Value,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                IsActive = appUser.IsActive,
                State = claims.SingleOrDefault(c => c.Type == ClaimNames.STATE)?.Value,
                City = claims.SingleOrDefault(c => c.Type == ClaimNames.CITY)?.Value,
                Address = claims.SingleOrDefault(c => c.Type == ClaimNames.ADDRESS)?.Value,
                CreationTime = appUser.CreationTime.ToString(Values.DATETIME_FORMAT)
            };
        }

        private async Task<string> generateTokenAsync(AppUser appUser) {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT.SecretKey));
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            var tokenClaims = new List<Claim> {
                        new Claim(ClaimNames.USER_ID, appUser.Id),
                        new Claim(ClaimNames.GIVEN_NAME, userClaims.SingleOrDefault(c => c.Type == ClaimNames.GIVEN_NAME)?.Value),
                        new Claim(ClaimNames.SURNAME, userClaims.SingleOrDefault(c => c.Type == ClaimNames.SURNAME)?.Value),
                        new Claim(ClaimNames.ISADMIN, (userClaims.SingleOrDefault(c => c.Type == ClaimNames.GROUP)?.Value == UserGroups.ADMINS).ToString()),
                        new Claim(ClaimNames.GROUP, userClaims.SingleOrDefault(c => c.Type == ClaimNames.GROUP)?.Value)
                    };

            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor {
                Issuer = _appSettings.JWT.Issuer,
                Audience = _appSettings.JWT.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.JWT.SessionTimeout),
                Subject = new ClaimsIdentity(tokenClaims),
                SigningCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateJwtSecurityToken(descriptor);
            var tokenText = handler.WriteToken(token);

            return tokenText;
        }
        #endregion
    }
}
