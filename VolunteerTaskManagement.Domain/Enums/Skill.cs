using System.ComponentModel;

namespace VolunteerTaskManagement.Domain.Enums
{
    public enum Skill
    {
        [Description("امداد و نجات")]
        Rescue = 1,

        [Description("اطفای حریق")]
        Firefighting = 2,

        [Description("پرستاری و بهداشت")]
        Nursing = 3,

        [Description("کمک های اولیه")]
        FirstAid = 4,

        [Description("بنایی")]
        Masonry = 10,

        [Description("سیمان کاری و گچ کاری")]
        Cementing = 11,

        [Description("کاشی و سرامیک")]
        Tiling = 12,

        [Description("آرماتوربندی")]
        RebarWork = 13,

        [Description("قالب بندی")]
        Formwork = 14,

        [Description("بتن ریزی")]
        Concreting = 15,

        [Description("اسکلت فلزی")]
        SteelStructure = 16,

        [Description("جوشکاری برق و گاز")]
        Welding = 17,

        [Description("لوله کشی آب و فاضلاب")]
        Plumbing = 18,

        [Description("سقف کاذب و کناف")]
        Drywall = 19,

        [Description("برق ساختمان")]
        Electrical = 20,

        [Description("برق صنعتی و ژنراتور")]
        IndustrialElectric = 21,

        [Description("تاسیسات حرارتی (موتورخانه)")]
        HVAC = 22,

        [Description("تعمیر و نگهداری الکترونیک")]
        ElectronicsRepair = 23,

        [Description("نصب و تعمیر کولر و چیلر")]
        ACRepair = 24,

        [Description("آشپزی صنعتی")]
        IndustrialCooking = 30,

        [Description("نانوایی")]
        Baking = 31,

        [Description("انبارداری و توزیع")]
        Warehousing = 32,

        [Description("خیاطی و تعمیر چادر")]
        Tailoring = 33,

        [Description("مکانیکی خودرو")]
        AutoMechanic = 40,

        [Description("مکانیکی موتورسیکلت")]
        MotorcycleMechanic = 41,

        [Description("برق خودرو")]
        AutoElectric = 42,

        [Description("تعمیر موتور پمپ آب")]
        PumpRepair = 43,

        [Description("دیزل ژنراتور")]
        DieselGenerator = 44,

        [Description("فناوری اطلاعات")]
        IT = 50,

        [Description("راه اندازی شبکه")]
        Networking = 51,

        [Description("اپراتوری بی سیم")]
        RadioOperator = 52,

        [Description("پهپاد")]
        Drone = 53,

        [Description("تعمیرات موبایل و رادیو")]
        MobileRepair = 54,

        [Description("رانندگی لیسانس")]
        CarDriving = 60,

        [Description("رانندگی پایه یک (کامیون)")]
        TruckDriving = 61,

        [Description("رانندگی جرثقیل و لودر")]
        HeavyMachine = 62,

        [Description("رانندگی لیفتراک")]
        Forklift = 63,

        [Description("جوشکاری آب")]
        UnderwaterWelding = 70,

        [Description("لاستیک سازی و تعمیر تایر")]
        TireRepair = 71,

        [Description("آهنگری")]
        Blacksmithing = 72,

        [Description("نجاری و مبلمان")]
        Carpentry = 73
    }
}