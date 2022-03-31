using EmprestimoBancario.Controllers;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq;
using Xunit;
using EmprestimoBancario;

namespace EmprestimoBancarioTests.Controllers
{
    public class LinhaDeCreditoControllerTest
    {
        [Fact, TestPriority(1)]
        [Trait("Criar", "1")]
        public void CriarUmaLinhaDeCreditoRetornandoSucesso()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controller = new(mockDb.Object);
            LinhaDeCredito linhaDeCredito = new()
            {
                Limite = 1000,
                EmpresaId = 1,
                Investimentos = new List<Investimento>() {
                    new Investimento() { InvestidorId = 1, Porcentagem = 25},
                    new Investimento() { InvestidorId = 2, Porcentagem = 25},
                    new Investimento() { InvestidorId = 3, Porcentagem = 25},
                    new Investimento() { InvestidorId = 4, Porcentagem = 25}
                },
            };

            // Act
            var data = controller.Criar(linhaDeCredito);

            // Assert
            var exc = Assert.IsType<CreatedResult>(data.Result);
            Assert.Equal("201", exc.StatusCode.ToString());
        }

        [Fact, TestPriority(2)]
        [Trait("Criar", "2")]
        public void CriarUmaLinhaDeCreditoRetornandoErroDeValidacao()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controller = new(mockDb.Object);
            LinhaDeCredito linhaDeCredito = new()
            {
                Limite = 1000,
                EmpresaId = 1
            };

            // Act
            var data = controller.Criar(linhaDeCredito);

            // Assert
            var exc = Assert.IsType<BadRequestObjectResult>(data.Result);
            Assert.Equal("400", exc.StatusCode.ToString());
            Assert.Equal("Um linha de crédito só pode ser criado caso exista investimentos", exc.Value);
        }

        [Fact, TestPriority(3)]
        [Trait("Criar", "3")]
        public void CriarUmaLinhaDeCreditoRetornandoErroDeExecucao()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controller = new(mockDb.Object);
            LinhaDeCredito linhaDeCredito = new()
            {
                Limite = 1000,
                EmpresaId = -1,
                Investimentos = new List<Investimento>() {
                    new Investimento() {InvestidorId = 1, Porcentagem = 100 }
                },
            };

            // Act
            var data = controller.Criar(linhaDeCredito);

            // Assert
            var exc = Assert.IsType<BadRequestObjectResult>(data.Result);
            Assert.Equal("400", exc.StatusCode.ToString());
            Assert.Equal("Erro Interno.", exc.Value);
        }

        [Fact, TestPriority(4)]
        [Trait("Listar", "1")]
        public void RetornarUmaListaComTodasAsLinhasDeCreditoComSucesso()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controller = new(mockDb.Object);

            // Act
            var data = controller.Listar();

            // Assert
            var exc = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal("200", exc.StatusCode.ToString());
            Assert.IsType<List<LinhaDeCredito>>(exc.Value);
        }

        [Fact, TestPriority(5)]
        [Trait("Listar", "2")]
        public void RetornarUmaListaComUmaLinhaDeCreditoComSucesso()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controller = new(mockDb.Object);

            // Act
            var data = controller.ListarPorId(5);

            // Assert
            var exc = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal("200", exc.StatusCode.ToString());
            Assert.IsType<List<LinhaDeCredito>>(exc.Value);
        }

    }
}
