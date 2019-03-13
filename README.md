# Sharper.StockImages

A basic package that allows stock image services to be searched, and images from them to be downloaded.

In its current state, it simply pulls a random image from the [Unsplash API](https://unsplash.com/developers). To use it yourself, clone the repo and replace "`XXXXXXXX`" with your Unsplash API Client Key [on line 30 of the UnsplashStockImageService class file](src/Sharper.StockImages/Services/UnsplashStockImageService.cs#L30). You can sign up for a key [here](https://unsplash.com/join).

## Usage

To get the URL for a random image:

```c#
public async Task<string> GetRandomImageUrl()
{
    UnsplashStockImageService stockService = new UnsplashStockImageService();
    StockImageModel imageDetails = await stockService.GetImage();
    return imageDetails.ImageEmbedUrl;
}
```

The `StockImageModel` contains several other helpful properties, such as a link to the image on Unsplash (`ImageServiceUrl`) and attribution to the original photographer (`CreatorUserName`, `CreatorName`, and `CreatorServiceUrl`).

## Features

- [ ] [Unsplash](https://unsplash.com/) service **(IN PROGRESS)**
- [ ] Interface to allow additional services to be added **(IN PROGRESS)**
- [ ] Nuget package
- [ ] Umbraco package