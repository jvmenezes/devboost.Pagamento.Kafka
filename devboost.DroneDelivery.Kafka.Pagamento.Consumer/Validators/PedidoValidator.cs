using FluentValidation;
using devboost.Kafka.Pagamentos.Consumer.Model;

namespace devboost.Kafka.Pagamentos.Consumer.Validators
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(c => c.IdPedido).NotEmpty().WithMessage("Preencha o campo 'Id' do pedido");
            RuleFor(c => c.StatusPagamento).IsInEnum().WithMessage("Preencha o Status Pagamento correto");
        }
    }
}