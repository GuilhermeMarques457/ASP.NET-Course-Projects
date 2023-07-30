using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using Service;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class StockServiceClass
    {
        private readonly IStockServiceSellCreater _stockServiceSellCreater;
        private readonly IStockServiceBuyCreater _stockServiceBuyCreater;
        private readonly IStockServiceSellGetter _stockServiceSellGetter;
        private readonly IStockServiceBuyGetter _stockServiceBuyGetter;
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly IStockRepository _stockRepository;
        private readonly IFixture _fixture;


        public StockServiceClass()
        {
            _fixture = new Fixture();
            _stockRepositoryMock = new Mock<IStockRepository>();

            _stockRepository = _stockRepositoryMock.Object;
            _stockServiceBuyGetter = new StockServiceBuyGetter(_stockRepository);
            _stockServiceSellGetter = new StockServiceSellGetter(_stockRepository);
            _stockServiceSellCreater = new StockServiceSellCreater(_stockRepository);
            _stockServiceBuyCreater = new StockServiceBuyCreater(_stockRepository);

        }

        #region BuyOrder

        [Fact]
        public async Task CreateBuyOrder_NullOrder_ToBeArgumentNullException()
        {
            BuyOrderRequest? buyOrderRequest = null;

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CreateBuyOrder_MinimumQtde_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)0)
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_MaxQtde_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001)
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_MinPrice_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, (uint)0)
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_MaxPrice_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, (uint)1000001)
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_StockSymbolNull_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_InvalidDate_ToBeArgumentException()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(1998, 03, 03))
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_ProperBuy_ToBeSucessfull()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();


            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            _stockRepositoryMock.Setup(temp => temp.BuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            BuyOrderResponse buyOrderResponseActual = await _stockServiceBuyCreater.CreateBuyOrder(buyOrderRequest);
            buyOrderResponseExpected.BuyOrderID = buyOrderResponseActual.BuyOrderID;

            buyOrderResponseActual.BuyOrderID.Should().NotBeEmpty();
            buyOrderResponseActual.Should().BeEquivalentTo(buyOrderResponseExpected);
        }


        #endregion

        #region GetAllBuyOrders

        [Fact]
        public async void GetAllBuyOrders_EmptyList_ToBeEmpty()
        {
            _stockRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());

            List<BuyOrderResponse> buyOrderResponses = await _stockServiceBuyGetter.GetBuyOrders();

            buyOrderResponses.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllBuyOrders_ProperList_ToBeSucessfull()
        {
            BuyOrder? buyOrder1 = _fixture.Build<BuyOrder>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();

            BuyOrder? buyOrder2 = _fixture.Build<BuyOrder>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();

            BuyOrder? buyOrder3 = _fixture.Build<BuyOrder>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();

            List<BuyOrder> buyOrders = new List<BuyOrder>()
            {
                buyOrder1,
                buyOrder2,
                buyOrder3
            };

            List<BuyOrderResponse> buyOrderResponsesExpected = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            _stockRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrderResponsesActual = await _stockServiceBuyGetter.GetBuyOrders();

            buyOrderResponsesActual.Should().NotBeNull();
            buyOrderResponsesActual.Should().BeEquivalentTo(buyOrderResponsesExpected);
        }


        #endregion

        #region SellOrder

        [Fact]
        public async Task CreateSellOrder_NullOrder_ToBeArgumentNullException()
        {
            SellOrderRequest? sellOrderRequest = null;

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CreateSellOrder_MinimumQtde_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)0)
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateSellOrder_MaxQtde_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001)
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateSellOrder_MinPrice_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, (uint)0)
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateSellrder_MaxPrice_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, (uint)1000001)
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateSellOrder_StockSymbolNull_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateSellOrder_InvalidDate_ToBeArgumentException()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(1998, 03, 03))
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            Func<Task> action = (async () =>
            {
                await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateBuyOrder_ProperSell_ToBeSucessfull()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();


            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();

            _stockRepositoryMock.Setup(temp => temp.SellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            SellOrderResponse sellOrderResponseActual = await _stockServiceSellCreater.CreateSellOrder(sellOrderRequest);
            sellOrderResponseExpected.SellOrderID = sellOrderResponseActual.SellOrderID;

            sellOrderResponseActual.SellOrderID.Should().NotBeEmpty();
            sellOrderResponseActual.Should().BeEquivalentTo(sellOrderResponseExpected);
        }


        #endregion

        #region GetAllSellOrders

        [Fact]
        public async void GetAllSellOrders_EmptyList_ToBeEmpty()
        {
            _stockRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());

            List<SellOrderResponse> sellOrderResponses = await _stockServiceSellGetter.GetSellOrders();

            sellOrderResponses.Should().BeEmpty();
        }

        [Fact]
        public async void GetAllSellOrders_ProperList_ToBeSucessfull()
        {
            SellOrder? sellOrder1 = _fixture.Build<SellOrder>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();

            SellOrder? sellOrder2 = _fixture.Build<SellOrder>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();

            SellOrder? sellOrder3 = _fixture.Build<SellOrder>()
                .With(temp => temp.Quantity, (uint)2345)
                .With(temp => temp.DateAndTimeOfOrder, new DateTime(2005, 03, 03))
                .With(temp => temp.Price, (uint)1402)
                .Create();

            List<SellOrder> sellOrders = new List<SellOrder>()
            {
                sellOrder1,
                sellOrder2,
                sellOrder3
            };

            List<SellOrderResponse> sellOrderResponsesExpected = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            _stockRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);

            List<SellOrderResponse> sellOrderResponsesActual = await _stockServiceSellGetter.GetSellOrders();

            sellOrderResponsesActual.Should().NotBeNull();
            sellOrderResponsesActual.Should().BeEquivalentTo(sellOrderResponsesExpected);
        }

        #endregion

    }
}

