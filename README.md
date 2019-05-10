# Checkout

## Assignment

Part 1:
Your company has decided to create a new line of business.  As a start to this effort, they’ve come to you to help develop a prototype.  It is expected that this prototype will be part of a beta test with some actual customers, and if successful, it is likely that the prototype will be expanded into a full product.

Your part of the prototype will be to develop a Web API that will be used by customers to manage a basket of items. The business describes the basic workflow is as follows:

This API will allow our users to set up and manage an order of items.  The API will allow users to add and remove items and change the quantity of the items they want.  They should also be able to simply clear out all items from their order and start again.

The functionality to complete the purchase of the items will be handled separately and will be written by a different team once this prototype is complete.

For the purpose of this exercise, you can assume there’s an existing data storage solution that the API will use, so you can either create stubs for that functionality or simply hold data in memory.

Feel free to make any assumptions whenever you are not certain about the requirements, but make sure your assumptions are made clear either through the design or additional documentation.

Part 2:
Create a client library that makes use of the API endpoints created in Part 1.  The purpose of this code to provide authors of client applications a simple framework to use in their applications.

## Introduction

Before starting, I would like to express my sincerest apologies for the delay in completing this task.
Naturally, the project could be improved (additional code documentation, optimizations here and there), however, I hope this suffices to progress to the next phase of the recruitment process.

## 1. Data Models

### 1.1. API Data models

These models are only used in the Web API.

#### 1.1.1. ItemResponse

An `ItemResponse` is the representation of a product inside a basket.

```csharp
public class ItemResponse
{
    public int Id{get; set;}

    public string Name {get; set;}

    public string Description {get; set;}

    public decimal Price {get; set;}
}
```
##### Validation

The data for `ItemResponse` must pass the following data validation:

* Name cannot be empty or null
* Description cannot be empty or null
* Price must be positive

#### 1.1.2. BasketItemResponse

A `BasketItemResponse` represents the product order inside a basket. The class includes the selected Product and the number of items that the client wishes to buy.

```csharp
public class BasketItemResponse
{
    public ItemResponse Item {get; set;}

    public int Count {get; set;}
}
```

##### Validation

The data for `BasketItemResponse` must pass the following data validation:

* Item cannot null
* Count must be positive

#### 1.1.3. BasketResponse

A `BasketResponse` represents the shopping list of a client. This represents the main model for this assignment. Most of the operations allowed in the Web API is to interact with the basket itself. 

```csharp
public class BasketResponse
{
    public int Id{get; set;}

    public List<BasketItemResponse> Items {get; set;} = new List<BasketItemResponse>();
}
```

##### Validation

The data for `BasketResponse` must pass the following data validation:

* Each BasketItemResponse will be validated individually according to `BasketItem` rules


### 1.2. Storage Data Models

These models are the equivalent of the API Data models, however, they only used when comunicating directly with *IStorage*.

|**API Data Model**|**Storage Model Equivalent**|
|:-:|:-:|
|`ItemResponse`|`Item`|
|`BasketItemResponse`|`BasketItem`|
|`BasketResponse`|`Basket`|

#### 1.2.1. Item
```csharp
public class Item
{
    public int Id{get; set;}

    public string Name {get; set;}

    public string Description {get; set;}

    public decimal Price {get; set;}
}
```

#### 1.2.2. BasketItem
```csharp
public class BasketItem
{
    public Item Item {get; set;}

    public int Count {get; set;}
}
```

#### 1.2.3. Basket
```csharp
public class Basket
{
    public int Id{get; set;}

    public Dictionary<int, BasketItemResponse> Items {get; set;} = new Dictionary<int, BasketItemResponse>();
}
```

## 2. Storage

For the purpose of this exercise, everything is stored in memory. Naturally, this storage could be easily replaced due to the fact that the controllers interact with *IStorage*.

### 2.1. IStorage

```csharp
Task<IEnumerable<Item>> GetItemsAsync();
Task<Item> GetItemAsync(int id);
Task<int> InitBasketAsync();
Task<bool> AddOrReplaceItemOnBasketAsync(int basketId, int itemId, int count);
Task<bool> RemoveItemFromBasket(int basketId, int itemId);
Task<IEnumerable<Basket>> GetBasketsAsync();  
Task<Basket> GetBasketAsync(int basketId);
Task<bool> BasketExistsAsync(int id);
Task<bool> AddOrReplaceBasketAsync(Basket basket);
Task<decimal> CheckoutAsync(int basketId);
Task<decimal> GetTotalPriceAsync(int basketId);
Task<bool> RemoveBasketAsync(int basketId);
Task<bool> ClearBascketAsync(int basketId);
Task<bool> AddOrUpdateItemAsync(Item item);
Task<bool> RemoveItemAsync(int id);
```

## 3. REST Api

The API can be tested using **Checkout.Client** or using the [Swagger UI](https://localhost:5001/swagger).

The documentation and the example values available in the Swagger UI could be improved with some extra documentation for ease of use.

## 4. Client

To test the REST Web API I have implemented a client that communicates with customers and their requests. The client can found under the **Checkout.Client**.

The following API is available:

* Items Controller related
```csharp
public async Task<IEnumerable<ItemResponse>> GetItems();

public async Task<ItemResponse> GetItem(int id);

public async Task<ItemResponse> AddItem(ItemResponse item);

public async Task<bool> DeleteItem(int id);
```

* Baskets Controller related methods
```csharp
public async Task<IEnumerable<BasketResponse>> GetBaskets();

public async Task<BasketResponse> GetBasket(int id);

public async Task<BasketItemResponse> AddBasket(BasketResponse basket);

public async Task<bool> DeleteBasket(int id);

public async Task<int> CreateEmptyBasket();

public async Task<decimal> CheckoutBasket(int id);

public async Task<decimal> ClearBasket(int id);

public async Task<bool> AddOrUpdateItemToBasket(int id, int itemId, int count);

public async Task<bool> RemoveItemFromBasket(int id, int itemId);

public async Task<decimal> BasketPrice(int id);

```