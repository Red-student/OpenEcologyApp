using Microsoft.EntityFrameworkCore;

namespace OpenEcologyApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(EcologyDbContext context)
        {
            try
            {
                // Проверяем, есть ли уже данные в таблице
                if (context.GrainHarvests.Any())
                {
                    return; // Если данные уже есть, выходим
                }

                // Добавляем тестовые данные по Казахстану
                var testData = new List<GrainHarvest>
                {
                    // 2020 год
                    new GrainHarvest { Year = 2020, Region = "Акмолинская область", Yield = 10.5, SownArea = 4500.0, GrossHarvest = 4725.0 },
                    new GrainHarvest { Year = 2020, Region = "Костанайская область", Yield = 9.8, SownArea = 4200.0, GrossHarvest = 4116.0 },
                    new GrainHarvest { Year = 2020, Region = "Северо-Казахстанская область", Yield = 11.2, SownArea = 4300.0, GrossHarvest = 4816.0 },
                    new GrainHarvest { Year = 2020, Region = "Павлодарская область", Yield = 8.9, SownArea = 3800.0, GrossHarvest = 3382.0 },
                    new GrainHarvest { Year = 2020, Region = "Восточно-Казахстанская область", Yield = 7.5, SownArea = 3500.0, GrossHarvest = 2625.0 },

                    // 2021 год
                    new GrainHarvest { Year = 2021, Region = "Акмолинская область", Yield = 8.7, SownArea = 4550.0, GrossHarvest = 3958.5 },
                    new GrainHarvest { Year = 2021, Region = "Костанайская область", Yield = 7.9, SownArea = 4250.0, GrossHarvest = 3357.5 },
                    new GrainHarvest { Year = 2021, Region = "Северо-Казахстанская область", Yield = 9.1, SownArea = 4350.0, GrossHarvest = 3958.5 },
                    new GrainHarvest { Year = 2021, Region = "Павлодарская область", Yield = 7.2, SownArea = 3850.0, GrossHarvest = 2772.0 },
                    new GrainHarvest { Year = 2021, Region = "Восточно-Казахстанская область", Yield = 6.8, SownArea = 3550.0, GrossHarvest = 2414.0 },

                    // 2022 год
                    new GrainHarvest { Year = 2022, Region = "Акмолинская область", Yield = 11.2, SownArea = 4600.0, GrossHarvest = 5152.0 },
                    new GrainHarvest { Year = 2022, Region = "Костанайская область", Yield = 10.5, SownArea = 4300.0, GrossHarvest = 4515.0 },
                    new GrainHarvest { Year = 2022, Region = "Северо-Казахстанская область", Yield = 12.1, SownArea = 4400.0, GrossHarvest = 5324.0 },
                    new GrainHarvest { Year = 2022, Region = "Павлодарская область", Yield = 9.8, SownArea = 3900.0, GrossHarvest = 3822.0 },
                    new GrainHarvest { Year = 2022, Region = "Восточно-Казахстанская область", Yield = 8.3, SownArea = 3600.0, GrossHarvest = 2988.0 },

                    // 2023 год
                    new GrainHarvest { Year = 2023, Region = "Акмолинская область", Yield = 9.8, SownArea = 4650.0, GrossHarvest = 4557.0 },
                    new GrainHarvest { Year = 2023, Region = "Костанайская область", Yield = 9.2, SownArea = 4350.0, GrossHarvest = 4002.0 },
                    new GrainHarvest { Year = 2023, Region = "Северо-Казахстанская область", Yield = 10.5, SownArea = 4450.0, GrossHarvest = 4672.5 },
                    new GrainHarvest { Year = 2023, Region = "Павлодарская область", Yield = 8.5, SownArea = 3950.0, GrossHarvest = 3357.5 },
                    new GrainHarvest { Year = 2023, Region = "Восточно-Казахстанская область", Yield = 7.9, SownArea = 3650.0, GrossHarvest = 2883.5 },

                    // 2024 год (прогноз)
                    new GrainHarvest { Year = 2024, Region = "Акмолинская область", Yield = 10.8, SownArea = 4700.0, GrossHarvest = 5076.0 },
                    new GrainHarvest { Year = 2024, Region = "Костанайская область", Yield = 10.2, SownArea = 4400.0, GrossHarvest = 4488.0 },
                    new GrainHarvest { Year = 2024, Region = "Северо-Казахстанская область", Yield = 11.5, SownArea = 4500.0, GrossHarvest = 5175.0 },
                    new GrainHarvest { Year = 2024, Region = "Павлодарская область", Yield = 9.5, SownArea = 4000.0, GrossHarvest = 3800.0 },
                    new GrainHarvest { Year = 2024, Region = "Восточно-Казахстанская область", Yield = 8.7, SownArea = 3700.0, GrossHarvest = 3219.0 }
                };

                context.GrainHarvests.AddRange(testData);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации данных: {ex.Message}");
                throw;
            }
        }
    }
} 