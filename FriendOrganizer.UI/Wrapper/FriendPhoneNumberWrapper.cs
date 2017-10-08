using FriendOrganizer.Model;
using System;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendPhoneNumberWrapper : ModelWrapper<FriendPhoneNumber>
    {
        public FriendPhoneNumberWrapper(FriendPhoneNumber model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Number):
                    if (string.Equals(Number, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid friends";
                    }
                    break;
            }
        }
    }
}

