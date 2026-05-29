using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Infrastructure.Persistence.Context;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Seed
{
    public static class VolunteerTaskManagementSeed
    {
        public static void SeedDatabase(IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var applicationContext = scope.ServiceProvider.GetRequiredService<VolunteerTaskManagementContext>();
            applicationContext.Database.Migrate();

            // Seed User (موجود)
            if (!applicationContext.Set<User>().Any())
            {
                applicationContext.Set<User>().Add(new User
                {
                    FirstName = "یاسمن",
                    LastName = "حاجی",
                    UserName = "یاسمن حاجی",
                    Email = "VolunteerTaskManagement@gmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEH73e3F38FPbP+qDJRo33JIHDw0cFo+EEHDRC+FRS2MTvE9+F1KJMATxPTmKTHEuqw==",
                    Role = "Admin",
                    CreateDate = DateTime.Now
                });
                applicationContext.SaveChanges();
            }

            // ------------------------------------------------------------
            // Seed Geographical Data (Province -> City -> Region -> Neighborhood)
            // ------------------------------------------------------------

            // 1. استان تهران
            var tehranProvince = applicationContext.Set<Province>().FirstOrDefault(p => p.Title == "تهران");
            if (tehranProvince == null)
            {
                tehranProvince = new Province { Title = "تهران" };
                applicationContext.Set<Province>().Add(tehranProvince);
                applicationContext.SaveChanges();
            }

            // 2. شهر تهران
            var tehranCity = applicationContext.Set<City>().FirstOrDefault(c => c.Title == "تهران" && c.ProvinceId == tehranProvince.Id);
            if (tehranCity == null)
            {
                tehranCity = new City { Title = "تهران", ProvinceId = tehranProvince.Id };
                applicationContext.Set<City>().Add(tehranCity);
                applicationContext.SaveChanges();
            }

            // 3. دیکشنری شامل نام منطقه و لیست محله‌های آن
            var regionsWithNeighborhoods = new Dictionary<string, List<string>>
            {
                ["منطقه ۱"] = ["ازگل", "اقدسیه", "الهيه", "امامزاده قاسم", "اوین", "باغ فردوس", "تجریش", "جماران", "چیذر", "دارآباد", "دربند", "درکه", "دزاشیب - جوزستان", "زعفرانیه", "سوهانک", "شهرک نفت", "شهرک محلاتی", "فرمانیه", "فرشته", "قیطریه", "کاشانک", "کامرانیه", "محمودیه", "نیاوران", "ولنجک"],
                ["منطقه ۲"] = ["برق آلستوم", "تهران ویلا", "ستارخان", "سعادت آباد", "شهرک غرب", "شهرک مخابرات", "شهرآرا", "صادقیه", "طرشت", "فرحزاد", "گیشا", "همایونشهر", "مرزداران"],
                ["منطقه ۳"] = ["اختیاریه", "پاسداران", "دروس", "دولت", "دیباجی", "جردن - ولیعصر", "سیدخندان", "ظفر", "قلهک", "میرداماد", "ونک"],
                ["منطقه ۴"] = ["بلوار پروین", "تهرانپارس", "حکیمیه", "سراج", "شمس آباد - مجیدیه", "شمیران نو", "علم و صنعت", "فرجام", "قنات کوثر", "لویزان - شیان", "مهران", "نارمک", "هروی", "هنگام"],
                ["منطقه ۵"] = ["آيت الله كاشانی", "اشرفی اصفهانی", "باغ فیض", "بلوار فردوس", "پونک", "جنت آباد", "حصارک", "سازمان برنامه", "شاهین", "شهران", "شهرزيبا", "شهرک آپادانا", "شهرک اکباتان", "شهرک اندیشه", "شهرک پرواز", "شهرک کوهسار", "شهرک نفت", "شهرک والفجر", "کن", "کوی ارم", "کوی بیمه"],
                ["منطقه ۶"] = ["آرژانتین - ساعی", "امیرآباد", "ایرانشهر", "بهجت آباد", "پارک لاله", "جنت - رفتگر", "دانشگاه تهران", "شریعتی", "شیراز", "عباس آباد", "فاطمی", "قائم مقام - سنائی", "قزل قلعه", "کشاورز غربی", "گاندی", "میدان جهاد", "میدان ولیعصر", "نصرت", "یوسف آباد"],
                ["منطقه ۷"] = ["اجاره دار", "ارامنه", "امجدیه - خاقانی", "باغ صبا - سهروردی", "بهار", "حشمتیه", "خواجه نصیر - حقوقی", "دبستان - مجیدیه", "سبلان", "عباس آباد - اندیشه", "قصر", "کاج", "کریمخان ", "مطهری", "نامجو", "نظام آباد", "نیلوفر - شهید قندی", "هفت تیر"],
                ["منطقه ۸"] = ["تسلیحات", "تهرانپارس", "دردشت", "زرکش", "فدک", "کرمان", "لشکر", "مجیدیه جنوبی", "مدائن", "نارمک", "وحیدیه", "هفت حوض"],
                ["منطقه ۹"] = ["استاد معین", "امامزاده عبدالله", "دکتر هوشیار", "سرآسیاب مهرآباد", "شمشیری", "شهید دستغیب", "فتح - صنعتی", "فرودگاه"],
                ["منطقه ۱۰"] = ["بریانک", "جی – شبیری", "جیحون", "دامپزشكی", "رودکی", "زنجان", "سلیمانی - تیموری", "کارون", "قزوین", "کمیل", "مالک اشتر", "نواب صفوی", "هاشمی", "هفت چنار"],
                ["منطقه ۱۱"] = ["آذربایجان", "آگاهی", "اسکندری", "امیریه", "انبارنفت", "جمالزاده - حشمت الدوله", "جمهوری", "خرمشهر", "راه آهن", "شیخ هادی", "عباسی", "فروزش - امیربهادر", "فلسطین - انقلاب", "قلمستان - برادران جوادیان", "مخصوص", "منیریه", "میدان حر", "هلال احمر"],
                ["منطقه ۱۲"] = ["آبشار", "ارگ پامنار", "امامزاده یحیی", "ایران", "بازار", "بهارستان", "تختی", "دروازه شمیران", "سنگلج", "شهید هرندی", "فردوسی", "قیام", "کوثر"],
                ["منطقه ۱۳"] = ["آشتیانی", "امامت", "پیروزی", "تهران نو", "حافظیه", "دهقان", "زاهد گیلانی", "زینبیه", "سرخه حصار", "شورا", "شهید اسدی", "صفا", "قاسم آباد", "نیروی هوایی"],
                ["منطقه ۱۴"] = ["13 آبان", "آهنگ", "آهنگران", "ابوذر", "بروجردی", "پرستار", "پیروزی", "تاکسیرانی", "جابری", "جوادیه", "چهارصد دستگاه", "خاوران", "دژکام", "دولاب", "شاهین", "شکوفه", "شکیب", "شیوا", "صد دستگاه", "فرزانه", "قصر فیروزه", "مینای", "نبی اکرم", "نیکنام"],
                ["منطقه ۱۵"] = ["ابوذر", "اتابک", "اسلام آباد - والفجر", "افسریه", "بروجردی - دهقان", "شوش", "شهرک رضویه", "طیب", "کیانشهر", "مسعودیه", "مشیریه", "مطهری", "مظاهری", "مینایی", "ولیعصر - بی سیم", "هاشم آباد"],
                ["منطقه ۱۶"] = ["باغ آذری", "تختی", "جوادیه", "خزانه", "شهرک بعثت", "علی آباد", "نازی آباد", "یاخچی آباد"],
                ["منطقه ۱۷"] = ["آذری", "ابوذر", "امامزاده حسن", "باغ خزانه", "بلور سازی", "جلیلی", "زمزم", "زهتابی", "فلاح", "گلچین", "مقدم", "وصفنارد", "یافت آباد"],
                ["منطقه ۱۸"] = ["17 شهریور", "بهداشت", "تولید دارو", "حسینی - فردوس", "خلیج فارس", "شاد آباد", "شمس آباد", "شهرک امام خمینی", "شهید رجایی", "صاحب الزمان", "صادقیه", "ولیعصر", "یافت آباد"],
                ["منطقه ۱۹"] = ["اسفندیاری و بستان", "اسماعیل آباد", "بوستان ولایت", "بهمنیار", "خانی آباد", "دولتخواه", "شریعتی", "شکوفه", "شهرک رسالت", "شهید کاظمی", "نعمت آباد"],
                ["منطقه ۲۰"] = ["13 آبان", "ابن بابویه", "استخر", "اقدسیه", "باروت کوبی", "تقی آباد", "جوانمرد قصاب", "حمزه آباد", "دولت آباد", "دیلمان", "سرتخت", "شهادت", "شهید بهشتی", "عباس آباد", "علایین", "فیروزآبادی", "حمزه آباد", "منصوریه منگل", "ولی آباد", "هاشم آباد"],
                ["منطقه ۲۱"] = ["باشگاه نفت", "تهرانسر", "چیتگر", "شهرک آزادی", "شهرک استقلال", "شهرک پاسداران", "شهرک دانشگاه تهران", "شهرک دریا", "شهرک شهرداری", "شهرک غزالی", "شهرک فرهنگیان", "وردآورد", "ویلا شهر"],
                ["منطقه ۲۲"] = ["آزاد شهر - پیکان شهر", "دهکده المپیک", "زیبا دشت", "شهرک دژبان", "شهرک راه آهن", "شهرک شهید باقری", "شهرک صنعتی شریف", "همت غرب"]
            };

            foreach (var regionItem in regionsWithNeighborhoods)
            {
                // جستجوی منطقه بر اساس Title و CityId
                var region = applicationContext.Set<Region>()
                    .FirstOrDefault(r => r.Title == regionItem.Key && r.CityId == tehranCity.Id);

                if (region == null)
                {
                    region = new Region { Title = regionItem.Key, CityId = tehranCity.Id };
                    applicationContext.Set<Region>().Add(region);
                    applicationContext.SaveChanges();
                }

                // اضافه کردن محله‌ها
                foreach (var neighborhoodTitle in regionItem.Value)
                {
                    var exists = applicationContext.Set<Neighborhood>()
                        .Any(n => n.Title == neighborhoodTitle && n.RegionId == region.Id);

                    if (!exists)
                    {
                        var neighborhood = new Neighborhood
                        {
                            Title = neighborhoodTitle,
                            RegionId = region.Id
                        };
                        applicationContext.Set<Neighborhood>().Add(neighborhood);
                    }
                }
            }

            applicationContext.SaveChanges();
        }
    }
}