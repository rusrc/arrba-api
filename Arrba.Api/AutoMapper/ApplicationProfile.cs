using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Arrba.Constants;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Extensions;
using Arrba.ImageLibrary;
using Arrba.ImageLibrary.ModelViews;
using AutoMapper;


namespace Arrba.Api.AutoMapper
{
    public class ApplicationProfile : Profile
    {
        bool UseFirabaseMapping => true;

        public ApplicationProfile()
        {
            CreateMap<AdImgDto, VehicleImageDto>()
                .ForMember(dest => dest.ImageStatus, opt => opt.MapFrom(m => m.ImageStatus.ToString()));

            CreateMap<UserPhone, PhoneDto>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(m => m.Number))
                .ForMember(dest => dest.PriorityStatus, opt => opt.MapFrom(m => m.PriorityStatus));
            
            CreateMap<User, ProfileDto>()
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(m => m.UserPhones.Select(phone => phone.Number) ?? null));

            CreateMap<AdVehicle, VehicleDto>()
                .ForMember(dest => dest.MapJsonCoordList, opt => opt.MapFrom(m => m.MapJsonCoordList ?? null))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(m => m.Services ?? null))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(m => m.DateExpired < DateTime.Now))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(m => m.Description.Left(100)))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(m => $"{m.Brand.Name} {(m.Model != null ? m.Model.Name : "")} {m.Year}"))
                .ForMember(dest => dest.MainImg, GetMainImageSmall)
                .ForMember(dest => dest.MainImgSuperSmall, GetMainImageSuperSmall)
                .ForMember(dest => dest.MainImgMiddle, GetMainImageMiddle)
                .ForMember(dest => dest.ImagePath, GetImagePath);

            CreateMap<AdVehicle, VehicleWithPropertyDto>()
                .ForMember(dest => dest.MapJsonCoordList, opt => opt.MapFrom(m => m.MapJsonCoordList ?? null))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(m => m.Services ?? null))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(m => m.DateExpired < DateTime.Now))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(m => m.Description.Left(100)))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(m => $"{m.Brand.Name} {(m.Model != null ? m.Model.Name : "")} {m.Year}"))
                .ForMember(dest => dest.SeoTitle, SeoTitle)
                .ForMember(dest => dest.ImagePath, GetImagePath)
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(m => m.DynamicPropertyAds ?? null))
                .ForMember(dest => dest.MainImg, GetMainImageMiddle)
                .ForMember(dest => dest.MainImgSuperSmall, GetMainImageSuperSmall)
                .ForMember(dest => dest.MainImgMiddle, GetMainImageMiddle)
                .ForMember(dest => dest.Images, GetImagePathes);

            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(m => m.Name))
                .ForMember(dest => dest.PropertyID, opt => opt.MapFrom(m => m.ID))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(m => m.ControlType))
                .ForMember(dest => dest.SelectOptions, opt => opt.MapFrom(m => m.SelectOptions ?? null));

            CreateMap<DynamicPropertyAdVehicle, PropertyDto>()
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(m => m.Property.Description ?? null))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(m => m.Property.ControlType.ToString()))
                .ForMember(dest => dest.PropertyStatus, opt => opt.MapFrom(m => m.Property.ActiveStatus.ToString()))
                //.ForMember(dest => dest.UnitMeasure, opt => opt.MapFrom(m => m.Property.UnitMeasure))
                .ForMember(dest => dest.PropertyValue, GetPropertyName);

            CreateMap<PropertyCateg, PropertyFilterDto>()
                .ForMember(dest => dest.AddToCard, opt => opt.MapFrom(m => m.AddToCard.ToString()))
                .ForMember(dest => dest.AddToFilter, opt => opt.MapFrom(m => m.AddToFilter.ToString()))
                .ForMember(dest => dest.ControlType, opt => opt.MapFrom(m => m.Property.ControlType.ToString()))
                .ForMember(dest => dest.PropertyActiveStatus, opt => opt.MapFrom(m => m.Property.ActiveStatus.ToString()))
                .ForMember(dest => dest.PropertyGroupName, opt => opt.MapFrom(m => m.Property.PropertyGroup.NameMultiLang ?? null))
                .ForMember(dest => dest.SelectOptions, MapSelectOptions);

            CreateMap<CategBrand, ItemFilterDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(m => false))
                .ForMember(dest => dest.ID, opt => opt.MapFrom(m => m.Brand.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(m => m.Brand.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(m => m.Brand.Status.ToString()))
                .ForMember(dest => dest.TypeOfItems, opt => opt.MapFrom(m => "BrandType"));

            CreateMap<ItemModel, ItemFilterDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(m => false))
                .ForMember(dest => dest.TypeOfItems, opt => opt.MapFrom(m => "ModelType"));

            CreateMap<City, CityDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(m => m.NameMultiLang));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(m => m.NameMultiLang));

            CreateMap<SuperCategory, SuperCategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(m => m.NameMultiLang))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(m => m.Categories));

            CreateMap<Category, CategoryWithBrandsDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(m => m.ID))
                .ForMember(dest => dest.CategoryAlias, opt => opt.MapFrom(m => m.Alias))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(m => m.Name))
                .ForMember(dest => dest.SuperCategoryId, opt => opt.MapFrom(m => m.SuperCategID))
                .ForMember(dest => dest.SuperCategoryName, opt => opt.MapFrom(m => m.SuperCateg.Name))
                .ForMember(dest => dest.CategoryFileName, opt => opt.MapFrom(m => m.FileName));

            CreateMap<CategType, TypeCategoryDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(m => m.ItemType.ID))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(m => m.ItemType.Name))
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(m => m.CategID));
        }


        #region For VehicleDto
        private void GetImagePath(IMemberConfigurationExpression<AdVehicle, VehicleDto, string> opt)
        {
            opt.MapFrom(m => !string.IsNullOrEmpty(m.FolderImgName)
                ? ImgFolderManager.GetRelativeItemFolderPath(m.CategID.ToString(), m.FolderImgName)
                : string.Empty);
        }
        private void GetMainImageSuperSmall(IMemberConfigurationExpression<AdVehicle, VehicleDto, string> opt)
        {
            if (UseFirabaseMapping)
            {
                opt.MapFrom(m => ImgManager.GetImgPathFireBase(m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX2));
            }
            else
            {
                opt.MapFrom(m => ImgManager.GetImgPath(m.CategID.ToString(), m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX));
            }
        }
        private void GetMainImageSmall(IMemberConfigurationExpression<AdVehicle, VehicleDto, string> opt)
        {
            if (UseFirabaseMapping)
            {
                opt.MapFrom(m => ImgManager.GetImgPathFireBase(m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX2));
            }
            else
            {
                opt.MapFrom(m => ImgManager.GetImgPath(m.CategID.ToString(), m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX));
            }

        }

        private void GetMainImageMiddle(IMemberConfigurationExpression<AdVehicle, VehicleDto, string> opt)
        {
            if (UseFirabaseMapping)
            {
                opt.MapFrom(m => ImgManager.GetImgPathFireBase(m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX2));
            }
            else
            {
                opt.MapFrom(m => ImgManager.GetImgPath(m.CategID.ToString(), m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX));
            }         
        }
        #endregion

        #region For VehicleWithPropertyDto

        private void GetImagePathes(IMemberConfigurationExpression<AdVehicle, VehicleWithPropertyDto, ICollection<object>> opt)
        {
            if (UseFirabaseMapping)
            {
                opt.MapFrom(m => ImgManagerFirebase.GetImages(m.ImgJson, m.FolderImgName, null));
            }
            else
            {
                opt.MapFrom(m => ImgManagerSkiaSharp.GetImages(m.ImgJson, m.FolderImgName, m.CategID.ToString()));
            }
        }

        private void GetImagePath(IMemberConfigurationExpression<AdVehicle, VehicleWithPropertyDto, string> opt)
        {
            opt.MapFrom(m => !string.IsNullOrEmpty(m.FolderImgName)
                ? ImgFolderManager.GetRelativeItemFolderPath(m.CategID.ToString(), m.FolderImgName)
                : string.Empty);
        }

        private void GetMainImageMiddle(IMemberConfigurationExpression<AdVehicle, VehicleWithPropertyDto, string> opt)
        {
            if (UseFirabaseMapping)
            {
                opt.MapFrom(m => ImgManager.GetImgPathFireBase(m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX2));
            }
            else
            {
                opt.MapFrom(m => ImgManager.GetImgPath(m.CategID.ToString(), m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX));
            }
        }

        private void GetMainImageSuperSmall(IMemberConfigurationExpression<AdVehicle, VehicleWithPropertyDto, string> opt)
        {
            if (UseFirabaseMapping)
            {
                opt.MapFrom(m => ImgManager.GetImgPathFireBase(m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX2));
            }
            else
            {
                opt.MapFrom(m => ImgManager.GetImgPath(m.CategID.ToString(), m.FolderImgName, m.ImgJsonObject, CONSTANT.MIDDLE_FILE_NAME_PREFIX));
            }
        }
        #endregion

        private void MapSelectOptions(IMemberConfigurationExpression<PropertyCateg, PropertyFilterDto, ICollection<SelectOption>> opt)
        {
            opt.MapFrom(
                d => d.Property.SelectOptions != null
                    ? d.Property.SelectOptions.Select(sp => new SelectOption
                    {
                        ID = sp.ID,
                        Name = sp.NameMultiLang,
                        PropertyID = sp.PropertyID,
                        MetaDate = sp.MetaDate
                    })
                    : null);
        }

        private static void SeoTitle(IMemberConfigurationExpression<AdVehicle, VehicleWithPropertyDto, string> opt)
        {
            opt.MapFrom(m => $"Купить {m.Categ.Name} {m.Brand.Name} {(m.Model != null ? m.Model.Name : "")} в {GetAccusativeNoun(m.City.Name)} {(m.Year != null ? $"{m.Year} года" : "")} {m.Price} {m.Currency.FullName}");
        }

        private static string GetAccusativeNoun(string word)
        {
            // TODO performence issue CyrNounCollection
            //var collection = new CyrNounCollection();
            //var strict = collection.Get(word, GetConditionsEnum.Strict);
            //return strict.Decline().Prepositional;
            return word;
        }

        private void GetPropertyName(IMemberConfigurationExpression<DynamicPropertyAdVehicle, PropertyDto, string> opt)
        {
            opt.MapFrom(m => m.Property.ControlType == ControlTypeEnum.Select && m.Property.SelectOptions != null
                ? m.Property.SelectOptions.SingleOrDefault(so => Regex.IsMatch(m.PropertyValue, $"^{so.ID}$")).NameMultiLang
                : m.Property.ControlType == ControlTypeEnum.CheckBox && m.PropertyValue == "1"
                    ? "Есть"
                    : m.PropertyValue);
        }
    }
}
