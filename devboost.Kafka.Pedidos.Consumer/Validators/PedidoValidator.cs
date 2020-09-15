using devboost.Kafka.Pedidos.Consumer.Model;
using FluentValidation;

namespace devboost.Kafka.Pedidos.Consumer.Validators
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(c => c.Peso).NotEmpty().WithMessage("Preencha o campo 'Peso' em gramas");     
        }
    }
}