using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Post
{
    public class PostDTOValidator : AbstractValidator<PostDTO>
    {

        public PostDTOValidator()
        {
            RuleFor(post => post.Title)
                .NotNull().WithMessage("El titulo no puede ser nulo.")
                .NotEmpty().WithMessage("El titulo no puede ser vacío.");
            RuleFor(post => post.Description)
                .NotNull().WithMessage("La descripción no puede ser nula.")
                .NotEmpty().WithMessage("La descripción no puede ser vacía.");
        }

    }
}
