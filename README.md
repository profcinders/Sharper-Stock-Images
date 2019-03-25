# Sharper.StockImages

[![Sharper.StockImages nuget package](https://buildstats.info/nuget/Sharper.StockImages?includePreReleases=true "Sharper.StockImages nuget package")](https://www.nuget.org/packages/Sharper.StockImages/)

A basic package that allows stock image services to be searched, and images from them to be downloaded.

In its current state, it simply pulls a random image from the [Unsplash API](https://unsplash.com/developers). To use it yourself, install the [nuget package](https://www.nuget.org/packages/Sharper.StockImages/) and add your Unsplash API Client Key to your `web.config` or `app.config` file (see code below). You can sign up for a key [here](https://unsplash.com/join).

```xml
<configuration>
  <appSettings>
    <add key="Sharper.StockImages.Unsplash.ClientId" value="YOUR_UNSPLASH_CLIENT_ID_GOES_HERE" />
  </appSettings>
</configuration>
```

## Usage

To get the URL for a random image:

```c#
public async Task<string> GetRandomImageUrl()
{
    UnsplashStockImageService stockService = new UnsplashStockImageService();
    StockImageModel imageDetails = await stockService.GetRandomImage();
    return imageDetails.ImageEmbedUrl;
}
```

And to get the URL for a specific image:

```c#
public async Task<string> GetImageUrl(string id)
{
    UnsplashStockImageService stockService = new UnsplashStockImageService();
    StockImageModel imageDetails = await stockService.GetImage(id);
    return imageDetails.ImageEmbedUrl;
}
```

The `StockImageModel` contains several helpful properties in addition to `ImageEmbedUrl` in the code samples, such as a link to the image on Unsplash (`ImageServiceUrl`) and attribution to the original photographer (`CreatorUserName`, `CreatorName`, and `CreatorServiceUrl`).

## Features

- [ ] [Unsplash](https://unsplash.com/) service **(IN PROGRESS)**
- [ ] Interface to allow additional services to be added **(IN PROGRESS)**
- [x] Nuget package
- [ ] Umbraco package