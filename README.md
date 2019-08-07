# Choose And Buy

This is a simple E-commerce web application.
Users can browse the products catalog and use their shopping cart.
For making an order a user needs to register.
Once registered a user can mark products as favorites, make orders and check their status anytime.

The first registered user is automatically assigned as Administrator.
He has all the user capabilities except one more - the Admin Pane.
The Admin Pane is the place where the orders are processed. 
Products, categories, sub-categories, user roles can be created, deleted and edited.

## Running the Project

Before running the project in Visual Studio, please change the following parameters for an optimal experience.
In the appsettings.json configure your API Keys and Secrets for:
- Cloudinary (storing the products images)
- Google     (external authentication)
- Facebook   (external authentication)
- SendGrid   (email sender for account confirmation)

At the GlobalConstants located in "ChooseAndBuy.Common.csproj" setup an email for the sender
and change the port in "ProductDetailsUrl" with yours.

Thats all! Hope you like It :ok_hand:
