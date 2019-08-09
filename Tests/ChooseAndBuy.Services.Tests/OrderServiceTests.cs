namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Common;
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.ViewModels.Administration.Orders;
    using ChooseAndBuy.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class OrderServiceTests
    {
        private async Task<List<OrderProductViewModel>> GetProductsForOrder()
        {
            var products = new List<OrderProductViewModel>()
            {
                new OrderProductViewModel
                {
                    Name = "product1",
                    TotalPrice = 10,
                },
                new OrderProductViewModel
                {
                    Name = "product2",
                    TotalPrice = 10,
                },
                new OrderProductViewModel
                {
                    Name = "product3",
                    TotalPrice = 10,
                },
            };

            return products;
        }

        private async Task<string> SeedSingleOrder(ApplicationDbContext context)
        {
            Order order = new Order
            {
                Id = "SingleOrder",
                Status = OrderStatus.Pending,
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return order.Id;
        }

        private async Task<string> SeedSingleUser(ApplicationDbContext context)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "TestUser",
                UserName = "TestUsername",
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user.Id;
        }

        private async Task SeedMultipleOrders(ApplicationDbContext context)
        {
            var orders = new List<Order>()
            {
                new Order
                {
                    Id = "first",
                    AdditionalInfo = "testFirst",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                },
                new Order
                {
                    Id = "second",
                    AdditionalInfo = "testSecond",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                },
                new Order
                {
                    Id = "third",
                    AdditionalInfo = "testThird",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.DeliveryInProgress,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                },
                new Order
                {
                    Id = "forth",
                    AdditionalInfo = "testForth",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.DeliveryInProgress,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                },
            };

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }

        private async Task<string> SeedOrderWithProducts(ApplicationDbContext context)
        {
            var orderId = string.Empty;

            // Seed Products
            var products = new List<Product>()
            {
                new Product
                {
                    Id = "firstProduct",
                    Name = "firstProductName",
                },
                new Product
                {
                    Id = "secondProduct",
                    Name = "secondProductName",
                },
                new Product
                {
                    Id = "thirdProduct",
                    Name = "thirdProductName",
                },
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // Seed Order
            Order order = new Order
            {
                DeliveryType = DeliveryType.Express,
                PaymentType = PaymentType.OnDelivery,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                DeliveryAddress = new Address
                {
                    AddressText = "TestAddress",
                    Id = "TestAddress",
                },
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            orderId = order.Id;

            // Seed Order Products
            var orderProducts = new List<OrderProduct>()
            {
                new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = "firstProduct",
                },
                new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = "secondProduct",
                },
                new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = "thirdProduct",
                },
            };

            await context.OrderProducts.AddRangeAsync(orderProducts);
            await context.SaveChangesAsync();

            return orderId;
        }

        private async Task<List<Order>> SeedUserWithMultipleOrders(ApplicationDbContext context)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "TestUser",
                UserName = "TestUsername",
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var orders = new List<Order>()
            {
                new Order
                {
                    Id = "first",
                    ApplicationUserId = user.Id,
                    AdditionalInfo = "testFirst",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                    DeliveryAddress = new Address
                    {
                        AddressText = "TestAddress",
                        Id = "TestAddress",
                    },
                },
                new Order
                {
                    Id = "second",
                    ApplicationUserId = user.Id,
                    AdditionalInfo = "testSecond",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                    DeliveryAddress = new Address
                    {
                        AddressText = "TestAddress",
                        Id = "TestAddress",
                    },
                },
                new Order
                {
                    Id = "third",
                    ApplicationUserId = user.Id,
                    AdditionalInfo = "testThird",
                    DeliveryType = DeliveryType.Express,
                    PaymentType = PaymentType.OnDelivery,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DispatchDate = DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays),
                    DeliveryAddress = new Address
                    {
                        AddressText = "TestAddress",
                        Id = "TestAddress",
                    },
                },
            };

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();

            return orders;
        }

        public OrderServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task CreateOrder_WithNoProducts_ShouldrReturnNull()
        {
            string onNotNullErrorMessage = "The method did not return null upon invalid input.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            OrderBindingModel model = new OrderBindingModel
            {
                DeliveryType = DeliveryType.Express,
                PaymentType = PaymentType.OnDelivery,
            };

            var orderId = await orderService.CreateOrder(model);

            AssertExtensions.NullWithMessage(orderId, onNotNullErrorMessage);
        }

        [Fact]
        public async Task CreateOrder_WithGivenProducts_ShouldrCreateOrderSuccessfully()
        {
            string onNullErrorMessage = "The order was not added to the database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            OrderBindingModel model = new OrderBindingModel
            {
                DeliveryType = DeliveryType.Express,
                PaymentType = PaymentType.OnDelivery,
                Products = await this.GetProductsForOrder(),
            };

            var orderId = await orderService.CreateOrder(model);

            var orderFromDatabase = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            AssertExtensions.NotNullWithMessage(orderFromDatabase, onNullErrorMessage);
        }

        [Fact]
        public async Task AddProductsToOrder_WithExistingOrderAndGivenProducts_ShouldAddProductsSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false with valid data.";
            string onCountDifferenceErrorMessage = "The returned order products don't match the expected count.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var products = await this.GetProductsForOrder();

            OrderBindingModel model = new OrderBindingModel
            {
                DeliveryType = DeliveryType.Express,
                PaymentType = PaymentType.OnDelivery,
                Products = products,
            };

            var orderId = await orderService.CreateOrder(model);

            var methodResult = await orderService.AddProductsToOrder(orderId, products);

            Assert.True(methodResult, onFalseErrorMessage);

            var orderFromDatabase = context.OrderProducts.Where(x => x.OrderId == orderId);

            var expectedCount = products.Count;

            AssertExtensions.EqualCountWithMessage(expectedCount, orderFromDatabase.Count(), onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task AddProductsToOrder_WithNonExistingOrderAndGivenProducts_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid orderId.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var products = await this.GetProductsForOrder();

            var orderId = "FakeOrderId";

            var methodResult = await orderService.AddProductsToOrder(orderId, products);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task AddProductsToOrder_WithExistingOrderAndNoProducts_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon no products input.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var products = await this.GetProductsForOrder();

            OrderBindingModel model = new OrderBindingModel
            {
                DeliveryType = DeliveryType.Express,
                PaymentType = PaymentType.OnDelivery,
                Products = products,
            };

            var orderId = await orderService.CreateOrder(model);

            // Passing null instead of the products
            var methodResult = await orderService.AddProductsToOrder(orderId, null);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task GetConfirmationInfo_WithExistingOrder_ShouldReturnModel()
        {
            string onNullErrorMessage = "The returned model is null.";
            string onStringDifference = "The model does not provide correct data for the order.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = await this.SeedOrderWithProducts(context);

            var confirmationModel = await orderService.GetConfirmationInfo(orderId);

            AssertExtensions.NotNullWithMessage(confirmationModel, onNullErrorMessage);

            var orderFromDatabase = await context.Orders
                .FirstOrDefaultAsync(x =>
                confirmationModel.QuantityProducts == x.Quantity &&
                confirmationModel.TotalPrice == x.TotalPrice &&
                confirmationModel.PaymentMethod == x.PaymentType.ToString());

            AssertExtensions.EqualStringWithMessage(orderId, orderFromDatabase.Id, onStringDifference);
        }

        [Fact]
        public async Task GetConfirmationInfo_WithNonExistingOrder_ShouldReturnNull()
        {
            string onNotNullErrorMessage = "The method did not return null upon non-existing order.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = "FakeOrderId";

            var confirmationModel = await orderService.GetConfirmationInfo(orderId);

            AssertExtensions.NullWithMessage(confirmationModel, onNotNullErrorMessage);
        }

        [Fact]
        public async Task GetAllUserOrders_WithSeededUserWithOrders_ShouldReturnCorrectOrders()
        {
            string onCountDifferenceErrorMessage = "The returned orders are not the same count as expected.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            // Seeding the orders for one user and recieves the orders.
            var orders = await this.SeedUserWithMultipleOrders(context);

            var userId = orders.First().ApplicationUserId;

            var methodResult = await orderService.GetAllUserOrders(userId);

            var expectedCount = orders.Count;
            var actualCount = methodResult.Count();

            AssertExtensions.EqualCountWithMessage(
                expectedCount,
                actualCount,
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetAllUserOrders_WithNonExistingUser_ShouldReturnAnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection with non-existing user.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var userId = "FakeUserId";

            var methodResult = await orderService.GetAllUserOrders(userId);

            AssertExtensions.EmptyWithMessage(methodResult, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetAllUserOrders_WithUserWithNoOrders_ShouldReturnAnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection with no user orders.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            // Seeding only one user with no orders
            var userId = await this.SeedSingleUser(context);

            var methodResult = await orderService.GetAllUserOrders(userId);

            AssertExtensions.EmptyWithMessage(methodResult, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetPendingOrders_WithSeededOrders_ShouldReturnCorrectOrders()
        {
            string onCountDifferenceErrorMessage = "The method did not return the correct orders.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            // Seeding multiple orders with 2 pending orders
            await this.SeedMultipleOrders(context);

            var pendingOrders = await orderService.GetPendingOrders();

            var expectedCount = 2;

            AssertExtensions.EqualCountWithMessage(
                expectedCount,
                pendingOrders.Count(),
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetPendingOrders_WithNoOrders_ShouldReturnAnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var pendingOrders = await orderService.GetPendingOrders();

            AssertExtensions.EmptyWithMessage(pendingOrders, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetActiveOrders_WithSeededOrders_ShouldReturnCorrectOrders()
        {
            string onCountDifferenceErrorMessage = "The method did not return the correct orders.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            // Seeding multiple orders with 2 active orders
            await this.SeedMultipleOrders(context);

            var pendingOrders = await orderService.GetActiveOrders();

            var expectedCount = 2;

            AssertExtensions.EqualCountWithMessage(
                expectedCount,
                pendingOrders.Count(),
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetActiveOrders_WithNoOrders_ShouldReturnAnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var pendingOrders = await orderService.GetActiveOrders();

            AssertExtensions.EmptyWithMessage(pendingOrders, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task ApproveOrder_WithExistingOrder_ShouldApproveSuccessfully()
        {
            string onFalseMethodResultErrorMessage = "The method did not return true on valid order.";
            string onFalseDatabaseResultErrorMessage = "The order status was not changed properly";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = await this.SeedSingleOrder(context);

            bool methodResult = await orderService.ApproveOrder(orderId);

            Assert.True(methodResult, onFalseMethodResultErrorMessage);

            var orderFromDatabase = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            Assert.True(orderFromDatabase.Status == OrderStatus.DeliveryInProgress, onFalseDatabaseResultErrorMessage);
        }

        [Fact]
        public async Task ApproveOrder_WithNonExistingOrder_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method did not return false on non-existing order.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = "FakeOrderId";

            bool methodResult = await orderService.ApproveOrder(orderId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task CancelOrder_WithExistingOrder_ShouldApproveSuccessfully()
        {
            string onFalseMethodResultErrorMessage = "The method did not return true on valid order.";
            string onFalseDatabaseResultErrorMessage = "The order status was not changed properly";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = await this.SeedSingleOrder(context);

            bool methodResult = await orderService.CancelOrder(orderId);

            Assert.True(methodResult, onFalseMethodResultErrorMessage);

            var orderFromDatabase = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            Assert.True(orderFromDatabase.Status == OrderStatus.Canceled, onFalseDatabaseResultErrorMessage);
        }

        [Fact]
        public async Task CancelOrder_WithNonExistingOrder_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method did not return false on non-existing order.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = "FakeOrderId";

            bool methodResult = await orderService.CancelOrder(orderId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task DeliverOrder_WithExistingOrder_ShouldApproveSuccessfully()
        {
            string onFalseMethodReturnErrorMessage = "The method did not return true on valid order.";
            string onFalseDatabaseReturnErrorMessage = "The order status was not changed properly";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = await this.SeedSingleOrder(context);

            bool methodResult = await orderService.DeliverOrder(orderId);

            Assert.True(methodResult, onFalseMethodReturnErrorMessage);

            var orderFromDatabase = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            Assert.True(orderFromDatabase.Status == OrderStatus.Delivered, onFalseDatabaseReturnErrorMessage);
        }

        [Fact]
        public async Task DeliverOrder_WithNonExistingOrder_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method did not return false on non-existing order.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var orderService = new OrderService(context);

            var orderId = "FakeOrderId";

            bool methodResult = await orderService.DeliverOrder(orderId);

            Assert.False(methodResult, onTrueErrorMessage);
        }
    }
}
