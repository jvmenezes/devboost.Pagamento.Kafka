using FluentValidation;

namespace devboost.DroneDelivery.Kafka.Pedido.Consumer.Validators
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(c => c.Peso).NotEmpty().WithMessage("Preencha o campo 'Peso' em gramas");     
        }
    }
}