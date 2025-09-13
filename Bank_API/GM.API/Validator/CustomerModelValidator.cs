//using FluentValidation;
//using GM.MODEL.Model.Customer;

//namespace GM.API.Validator
//{
//    public class CustomerModelValidator : AbstractValidator<CustomerModel>
//    {
//        public CustomerModelValidator()
//        {
//            RuleFor(x => x.Name)
//                .NotEmpty().WithMessage("Customer Name is required.")
//                .MaximumLength(254).WithMessage("Customer name cannot exceed 254 characters.");

//            RuleFor(x => x.RouteSaleCode)
//                .NotEmpty().WithMessage("RouteSaleCode is required.")
//                .MaximumLength(50).WithMessage("RouteSaleCode cannot exceed 50 characters.");

//            RuleFor(x => x.SaleUserCode)
//                .NotEmpty().WithMessage("SaleUserCode is required.")
//                .MaximumLength(50).WithMessage("SaleUserCode cannot exceed 50 characters.");

//        }
//    }

//}


