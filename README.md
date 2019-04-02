# Sharper.StockImages

[![Sharper.StockImages nuget package](https://buildstats.info/nuget/Sharper.StockImages?includePreReleases=true "Sharper.StockImages nuget package")](https://www.nuget.org/packages/Sharper.StockImages/)

A basic package that allows stock image services to be searched, and images from them to be downloaded.

Only [Unsplash](https://unsplash.com/) images are included by default, but additional services can be added by implementing the [IStockImageService](src/Sharper.StockImages/Services/IStockImageService.cs) interface, as described in the [Adding Custom Services](#adding-custom-services) section of this page.

## Installation

Sharper Stock Images can be installed [via nuget](https://www.nuget.org/packages/Sharper.StockImages/) or by building manually from the source code.

After installation, in order to call the [Unsplash API](https://unsplash.com/developers), an appropriate API Client Key needs to be added to the `web.config` or `app.config` app settings as shown below. You can sign up for a key [here](https://unsplash.com/join).

```xml
<configuration>
  <appSettings>
    <add key="Sharper.StockImages.Unsplash.ClientId" value="YOUR_UNSPLASH_CLIENT_ID_GOES_HERE" />
  </appSettings>
</configuration>
```

## Usage

### Stock Image Provider

#### Instantiation

```c#
var provider = new StockImageProvider();
var specificServicesProvider = new StockImageProvider(stockService1, stockService2, ...);
```

The initial call will search the loaded assemblies for classes that implement the `IStockImageService`, and add them to the `Services` property to search in future. Any services that have been disabled via a `DisableServiceAttribute` will not be added.

The second call takes a list of specific services to load, and no other services will be included. Any disabling attributes will be ignored in this case.

The `Services` list cannot be modified after instantiation, so make sure all the services you need are included!

#### Random image details

```c#
var provider = new StockImageProvider();
StockImageModel imageDetails = await provider.GetRandomImage();
```

This will query a service chosen at random, via its own `GetRandomImage()` method. If needed, a `Random` instance can be passed to this method.

#### Specific image details

```c#
var provider = new StockImageProvider();
StockImageModel imageDetails = await provider.GetImage("specific_image_id", "service_id");
```

Note that this method takes a service ID in addition to the image ID. This is to ensure that the correct service is queried for the image details.

### Unsplash Service

#### Random image details

```c#
UnsplashStockImageService stockService = new UnsplashStockImageService();
StockImageModel imageDetails = await stockService.GetRandomImage();
```

#### Specific image details

```c#
UnsplashStockImageService stockService = new UnsplashStockImageService();
StockImageModel imageDetails = await stockService.GetImage(id);
```

### Image Details Object

The `StockImageModel` contains several helpful properties. The main ones are the service ID (`ServiceId`) and the image ID (`Id`), along with the URL used to embed the image directly (`ImageEmbedUrl`). There are also properties such as links back to the image on the service's website, and attributions to the creator of the image. These may not be filled in on every service, however.

When querying the provider, the image ID and service ID are both important, as the provider will need to know which service to query for the image in any future calls.

## Adding Custom Services

To create your own stock image service, simply create a class that implements the `IStockImageService` interface. The service must have a unique `Id` to differentiate it to other services when called via the provider. The availability of random image functionality should be indicated with the `RandomImageEnabled` property.

If the service should be used in place of another (or a number of others), these can be disabled by using a `DisableServiceAttribute` specifying either the `Id` of the service to disable, or its Type.

## Features

- [ ] [Unsplash](https://unsplash.com/) service **(IN PROGRESS)**
- [ ] Interface to allow additional services to be added **(IN PROGRESS)**
- [x] Nuget package
- [ ] Umbraco package