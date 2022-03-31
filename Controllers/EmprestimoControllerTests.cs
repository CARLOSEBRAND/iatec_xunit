using EmprestimoBancario;
using EmprestimoBancario.Controllers;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EmprestimoBancarioTests.Controllers
{
    public class EmprestimoControllerTest
    {
        [Fact, TestPriority(12)]
        [Trait("Listar", "1")]
        public void Ti_RetornarListaDeTodosOsEmprestimosComSucesso()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            EmprestimoController controller = new(mockDb.Object);

            // Act
            var data = controller.Listar();

            // Assert
            var exc = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal("200", exc.StatusCode.ToString());
            Assert.IsType<List<Emprestimo>>(exc.Value);
        }

        [Fact, TestPriority(13)]
        [Trait("Listar", "2")]
        public void Tj_RetornarUmEmprestimoComSucesso()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            EmprestimoController controller = new(mockDb.Object);

            // Act
            var data = controller.ListarPorId(5);

            // Assert
            var exc = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal("200", exc.StatusCode.ToString());
            Assert.IsType<List<Emprestimo>>(exc.Value);
        }

        [Fact, TestPriority(6)]
        [Trait("Criar", "1")]
        public void Ta_CriarUmEmprestimoRetornandoSucesso()
        {
            // Arrange
            int id = CenarioTeste1("lc");
            var mockDb = new Mock<BancoDeDadosContexto>();
            EmprestimoController controller = new(mockDb.Object);

            // Act
            Emprestimo emprestimo = new() { Quantia = 500, LinhaDeCreditoId = id };
            var emp = controller.Criar(emprestimo);

            // Assert
            var exc = Assert.IsType<CreatedResult>(emp.Result);
            Assert.Equal("201", exc.StatusCode.ToString());
        }

        [Fact, TestPriority(7)]
        [Trait("Criar", "2")]
        public void Tb_CriarUmEmprestimoRetornandoErroDeValidacao()
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            EmprestimoController controller = new(mockDb.Object);
            Emprestimo emprestimo = new() { Quantia = 500 };

            // Act
            var data = controller.Criar(emprestimo);

            // Assert
            var exc = Assert.IsType<BadRequestObjectResult>(data.Result);
            Assert.Equal("400", exc.StatusCode.ToString());
            Assert.Equal("A linha de crédito é obrigatória.", exc.Value);
        }

        [Fact, TestPriority(8)]
        [Trait("Aprovar", "1")]
        public void Tc_AprovarUmEmprestimoRetornandoSucesso()
        {
            // Arrange
            int id = CenarioTeste1("emp");
            var mockDb = new Mock<BancoDeDadosContexto>();
                        
            EmprestimoController controller = new(mockDb.Object);
            AprovarEmprestimoModel aprovarEmprestimo = new() { InvestidorId = 1, Porcentagem = 25, Confirma = 'S' };

            // Act
            var data = controller.Aprovar(id, aprovarEmprestimo);

            // Assert
            var exc = Assert.IsType<NoContentResult>(data.Result);
            Assert.Equal("204", exc.StatusCode.ToString());
        }

        [Fact, TestPriority(9)]
        [Trait("Aprovar", "2")]
        public void Td_AprovarUmEmprestimoRetornandoErroDeValidacao()
        {
            // Arrange
            int id = CenarioTeste1("emp");
            var mockDb = new Mock<BancoDeDadosContexto>();

            EmprestimoController controller = new(mockDb.Object);
            AprovarEmprestimoModel aprovarEmprestimo = new() { InvestidorId = 1, Porcentagem = 70, Confirma = 'S' };

            // Act
            var data = controller.Aprovar(id, aprovarEmprestimo);

            // Assert
            var exc = Assert.IsType<BadRequestObjectResult>(data.Result);
            Assert.Equal("400", exc.StatusCode.ToString());
            Assert.Equal("O limite de investimento para esta linha de crédito é de até 25%.", exc.Value);
        }

        [Fact, TestPriority(10)]
        [Trait("Aprovar", "3")]
        public void Te_AprovarTodososEmprestimosComSucesso()
        {
            // Arrange
            int id = CenarioTeste1("emp");
            var mockDb = new Mock<BancoDeDadosContexto>();

            EmprestimoController controller = new(mockDb.Object);
            AprovarEmprestimoModel aprovarEmprestimo1 = new() { InvestidorId = 1, Porcentagem = 0, Confirma = 'N' };
            AprovarEmprestimoModel aprovarEmprestimo2 = new() { InvestidorId = 2, Porcentagem = 30, Confirma = 'S' };
            AprovarEmprestimoModel aprovarEmprestimo3 = new() { InvestidorId = 3, Porcentagem = 45, Confirma = 'S' };
            AprovarEmprestimoModel aprovarEmprestimo4 = new() { InvestidorId = 4, Porcentagem = 25, Confirma = 'S' };

            // Act and assert
            var data1 = controller.Aprovar(id, aprovarEmprestimo1);
            var exc1 = Assert.IsType<NoContentResult>(data1.Result);
            Assert.Equal("204", exc1.StatusCode.ToString());

            var data2 = controller.Aprovar(id, aprovarEmprestimo2);
            var exc2 = Assert.IsType<NoContentResult>(data2.Result);
            Assert.Equal("204", exc2.StatusCode.ToString());

            var data3 = controller.Aprovar(id, aprovarEmprestimo3);
            var exc3 = Assert.IsType<NoContentResult>(data3.Result);
            Assert.Equal("204", exc3.StatusCode.ToString());

            var data4 = controller.Aprovar(id, aprovarEmprestimo4);
            var exc4 = Assert.IsType<NoContentResult>(data4.Result);
            Assert.Equal("204", exc4.StatusCode.ToString());
        }

        [Fact, TestPriority(11)]
        [Trait("Status", "1")]
        public void Tf_VerificarStatusEmprestimoRetornandoSucesso()
        {
            // Arrange
            int id = CenarioTeste1("emp");
            var mockDb = new Mock<BancoDeDadosContexto>();

            EmprestimoController controller = new(mockDb.Object);
            AprovarEmprestimoModel aprovarEmprestimo1 = new() { InvestidorId = 1, Porcentagem = 0, Confirma = 'N' };
            AprovarEmprestimoModel aprovarEmprestimo2 = new() { InvestidorId = 2, Porcentagem = 35, Confirma = 'S' };
            AprovarEmprestimoModel aprovarEmprestimo3 = new() { InvestidorId = 3, Porcentagem = 0, Confirma = 'N' };
            AprovarEmprestimoModel aprovarEmprestimo4 = new() { InvestidorId = 4, Porcentagem = 65, Confirma = 'S' };

            // Act 
            controller.Aprovar(id, aprovarEmprestimo1);
            controller.Aprovar(id, aprovarEmprestimo2);
            controller.Aprovar(id, aprovarEmprestimo3);
            controller.Aprovar(id, aprovarEmprestimo4);
            var data = controller.VerificaStatus(id);

            // Assert
            var exc = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal("200", exc.StatusCode.ToString());
            Assert.Equal("Empréstimo Aprovado.", exc.Value);
        }


        [Fact, TestPriority(14)]
        [Trait("Aumentar", "1")]
        public void Th_AumentarOValorDeUmEmprestimoRetornandoSucesso()
        {
            // Arrange
            int id = CenarioTeste2("emp");
            var mockDb = new Mock<BancoDeDadosContexto>();

            EmprestimoController controller = new(mockDb.Object);
            AprovarEmprestimoModel aprovarEmprestimo1 = new() { InvestidorId = 3, Porcentagem = 45, Confirma = 'S' };
            AprovarEmprestimoModel aprovarEmprestimo2 = new() { InvestidorId = 2, Porcentagem = 55, Confirma = 'S' };

            controller.Aprovar(id, aprovarEmprestimo1);
            controller.Aprovar(id, aprovarEmprestimo2);
            controller.VerificaStatus(id);
                                    
            PagarEmprestimoModel pagarEmprestimo = new() { Quantia = 100, Taxa = 2.75 };

            // Act
            var data = controller.Aumentar(id, pagarEmprestimo);

            // Assert
            var exc = Assert.IsType<NoContentResult>(data.Result);
            Assert.Equal("204", exc.StatusCode.ToString());
        }

        [Fact, TestPriority(15)]
        [Trait("Diminuir", "1")]
        public void Tg_PagarUmValorDeEmprestimoRetornandoSucesso()
        {
            // Arrange
            int id = CenarioTeste2("emp");
            var mockDb = new Mock<BancoDeDadosContexto>();

            EmprestimoController controller = new(mockDb.Object);
            AprovarEmprestimoModel aprovarEmprestimo1 = new() { InvestidorId = 3, Porcentagem = 0, Confirma = 'N' };
            AprovarEmprestimoModel aprovarEmprestimo2 = new() { InvestidorId = 2, Porcentagem = 100, Confirma = 'S' };

            controller.Aprovar(id, aprovarEmprestimo1);
            controller.Aprovar(id, aprovarEmprestimo2);
            controller.VerificaStatus(id);

            PagarEmprestimoModel pagarEmprestimo = new() { Quantia = 300, Taxa = 10 };

            // Act
            var data = controller.Diminuir(id, pagarEmprestimo);

            // Assert
            var exc = Assert.IsType<NoContentResult>(data.Result);
            Assert.Equal("204", exc.StatusCode.ToString());
        }


        private static int CenarioTeste1(string tipoRetorno)
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controllerLc = new(mockDb.Object);
            EmprestimoController controllerEmp = new(mockDb.Object);

            LinhaDeCredito linhaDeCredito = new()
            {
                Limite = 1000,
                EmpresaId = 2,
                Investimentos = new List<Investimento>() {
                    new Investimento() { InvestidorId = 1, Porcentagem = 25},
                    new Investimento() { InvestidorId = 2, Porcentagem = 25},
                    new Investimento() { InvestidorId = 3, Porcentagem = 25},
                    new Investimento() { InvestidorId = 4, Porcentagem = 25}
                },
            };

            var lc = controllerLc.Criar(linhaDeCredito);
            var exc1 = Assert.IsType<CreatedResult>(lc.Result);

            int linhaDeCreditoId = (int)exc1.Value;

            Emprestimo emprestimo = new() { Quantia = 500, LinhaDeCreditoId = linhaDeCreditoId };
            var emp = controllerEmp.Criar(emprestimo);
            var exc2 = Assert.IsType<CreatedResult>(emp.Result);

            int emprestimoId = (int)exc2.Value; 

            if (tipoRetorno == "lc") {
                return linhaDeCreditoId;
            } else {
                return emprestimoId;
            }
        }

        private static int CenarioTeste2(string tipoRetorno)
        {
            // Arrange
            var mockDb = new Mock<BancoDeDadosContexto>();
            LinhaDeCreditoController controllerLc = new(mockDb.Object);
            EmprestimoController controllerEmp = new(mockDb.Object);

            LinhaDeCredito linhaDeCredito = new()
            {
                Limite = 7000,
                EmpresaId = 3,
                Investimentos = new List<Investimento>() {
                    new Investimento() { InvestidorId = 3, Porcentagem = 50},
                    new Investimento() { InvestidorId = 2, Porcentagem = 50}                    
                },
            };

            var lc = controllerLc.Criar(linhaDeCredito);
            var exc1 = Assert.IsType<CreatedResult>(lc.Result);

            int linhaDeCreditoId = (int)exc1.Value;

            Emprestimo emprestimo = new() { Quantia = 3000, LinhaDeCreditoId = linhaDeCreditoId };
            var emp = controllerEmp.Criar(emprestimo);
            var exc2 = Assert.IsType<CreatedResult>(emp.Result);

            int emprestimoId = (int)exc2.Value;

            if (tipoRetorno == "lc")
            {
                return linhaDeCreditoId;
            }
            else
            {
                return emprestimoId;
            }
        }

    }
}
