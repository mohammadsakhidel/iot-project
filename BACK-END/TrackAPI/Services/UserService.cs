using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using TrackAPI.Constants;
using TrackAPI.Helpers;
using TrackAPI.Models;
using TrackDataAccess.Models.Identity;

namespace TrackAPI.Services {
    public class UserService : IUserService {

        private UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        public async Task<string> CreateAsync(UserModel model) {

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                var user = new AppUser {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
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

                await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddClaimsAsync(user, claims);
                transaction.Complete();

                return user.Id;
            }

        }

        public async Task<UserModel> GetAsync(string id) {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null) {
                var claims = await _userManager.GetClaimsAsync(appUser);
                return mapAppUserToUserModel(appUser, claims);
            } else {
                return null;
            }
        }

        public async Task<List<UserModel>> GetAsync(int skip, int take) {
            var appUsers = await Task<AppUser>.Run(() => {
                return _userManager.Users.Skip(skip).Take(take).ToList();
            });

            var users = await Task<List<UserModel>>.Run(() => {
                var list = new List<UserModel>();
                foreach (var user in appUsers) {
                    var claims = _userManager.GetClaimsAsync(user).Result;
                    list.Add(mapAppUserToUserModel(user, claims));
                }
                return list;
            });

            return users;
        }

        public async Task<(bool, string)> RemoveAsync(string id) {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, "User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.FirstOrDefault()?.Description);

            return (true, string.Empty);
        }

        #region Private Methods:
        private UserModel mapAppUserToUserModel(AppUser appUser, IEnumerable<Claim> claims) {
            return new UserModel {
                GivenName = claims.SingleOrDefault(c => c.Type == ClaimNames.GIVEN_NAME)?.Value,
                Surname = claims.SingleOrDefault(c => c.Type == ClaimNames.SURNAME)?.Value,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                State = claims.SingleOrDefault(c => c.Type == ClaimNames.STATE)?.Value,
                City = claims.SingleOrDefault(c => c.Type == ClaimNames.CITY)?.Value,
                Address = claims.SingleOrDefault(c => c.Type == ClaimNames.ADDRESS)?.Value
            };
        }
        #endregion
    }
}
