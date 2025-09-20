using be_atoutmajeur.Enums;

namespace be_atoutmajeur.Models.Constant;

public static class GarmentPricing
{
    private static readonly Dictionary<GarmentType, decimal> _basePrices = new()
    {
        // Tops - 8€ à 15€
        { GarmentType.Shirts, 12.00m },
        { GarmentType.Tshirts, 8.00m },
        { GarmentType.PoloShirts, 10.00m },
        { GarmentType.Blouses, 14.00m },
        { GarmentType.Sweaters, 15.00m },
        { GarmentType.Hoodies, 15.00m },
        { GarmentType.TankTops, 8.00m },
        { GarmentType.Cardigans, 15.00m },

        // Bottoms - 10€ à 18€
        { GarmentType.Pants, 15.00m },
        { GarmentType.Jeans, 18.00m },
        { GarmentType.Shorts, 10.00m },
        { GarmentType.Skirts, 12.00m },
        { GarmentType.Leggings, 10.00m },
        { GarmentType.Trousers, 16.00m },

        // Dresses & Jumpsuits - 20€ à 35€
        { GarmentType.Dresses, 25.00m },
        { GarmentType.EveningDresses, 35.00m },
        { GarmentType.CocktailDresses, 30.00m },
        { GarmentType.CasualDresses, 20.00m },
        { GarmentType.Jumpsuits, 28.00m },
        { GarmentType.Rompers, 20.00m },

        // Outerwear - 20€ à 45€
        { GarmentType.Suits, 45.00m },
        { GarmentType.Blazers, 25.00m },
        { GarmentType.Jackets, 22.00m },
        { GarmentType.Coats, 30.00m },
        { GarmentType.WinterCoats, 35.00m },
        { GarmentType.LeatherJackets, 40.00m },
        { GarmentType.Windbreakers, 20.00m },
        { GarmentType.Vests, 18.00m },

        // Underwear & Intimate - 5€ à 12€
        { GarmentType.Underwear, 5.00m },
        { GarmentType.Bras, 12.00m },
        { GarmentType.Socks, 3.00m },
        { GarmentType.Stockings, 6.00m },
        { GarmentType.Lingerie, 15.00m },
        { GarmentType.Boxers, 5.00m },

        // Sportswear - 10€ à 18€
        { GarmentType.Sportswear, 15.00m },
        { GarmentType.YogaPants, 12.00m },
        { GarmentType.SportsBras, 12.00m },
        { GarmentType.Swimwear, 18.00m },
        { GarmentType.AthleticShorts, 10.00m },
        { GarmentType.GymWear, 15.00m },

        // Sleepwear - 8€ à 15€
        { GarmentType.Pajamas, 15.00m },
        { GarmentType.Nightgowns, 18.00m },
        { GarmentType.Robes, 20.00m },
        { GarmentType.SleepShirts, 8.00m },

        // Home Textiles - 6€ à 25€
        { GarmentType.BedSheets, 12.00m },
        { GarmentType.Pillowcases, 6.00m },
        { GarmentType.Towels, 8.00m },
        { GarmentType.BathTowels, 10.00m },
        { GarmentType.Blankets, 20.00m },
        { GarmentType.Comforters, 25.00m },
        { GarmentType.Curtains, 15.00m },
        { GarmentType.TableLinens, 12.00m },
        { GarmentType.DuvetCovers, 18.00m },

        // Special Occasion - 50€ à 80€
        { GarmentType.WeddingDresses, 80.00m },
        { GarmentType.FormalWear, 50.00m },
        { GarmentType.Uniforms, 20.00m },
        { GarmentType.Costumes, 25.00m },

        // Accessories - 5€ à 15€
        { GarmentType.Ties, 8.00m },
        { GarmentType.Scarves, 10.00m },
        { GarmentType.Belts, 12.00m },
        { GarmentType.Hats, 10.00m },
        { GarmentType.Gloves, 8.00m },

        // Other - 10€ à 30€
        { GarmentType.Other, 15.00m },
        { GarmentType.DelicateItems, 25.00m },
        { GarmentType.VintageItems, 30.00m },
        { GarmentType.SpecialtyFabrics, 28.00m },
        { GarmentType.BabyClothes, 8.00m },
        { GarmentType.ChildrenClothes, 10.00m }
    };
    
    public static decimal CalculatePrice(GarmentType garmentType, int quantity)
    {
        if (quantity <= 0) return 0m;
        
        var unitPrice = _basePrices.TryGetValue(garmentType, out var price) ? price : 15.00m;
        var totalPrice = unitPrice * quantity;
        
        return Math.Round(totalPrice, 2);
    }
}