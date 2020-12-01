using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAdmin.DTOs;

namespace TrackAdmin.ViewModels {
    public interface IUserEditorViewModel : IViewModel {
        public void Init(UserDto user);
    }
}
