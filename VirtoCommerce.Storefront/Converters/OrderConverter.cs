﻿using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Order;
using coreDto = VirtoCommerce.Storefront.AutoRestClients.CoreModuleApi.Models;
using orderDto = VirtoCommerce.Storefront.AutoRestClients.OrdersModuleApi.Models;
using platformDto = VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;
using storeDto = VirtoCommerce.Storefront.AutoRestClients.StoreModuleApi.Models;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OrderConverterExtension
    {
        public static OrderConverter OrderConverterInstance
        {
            get
            {
                return new OrderConverter();
            }
        }

        public static CustomerOrder ToCustomerOrder(this orderDto.CustomerOrder orderDto, ICollection<Currency> availCurrencies, Language language)
        {
            return OrderConverterInstance.ToCustomerOrder(orderDto, availCurrencies, language);
        }

        public static TaxDetail ToTaxDetail(this orderDto.TaxDetail taxDetailDto, Currency currency)
        {
            return OrderConverterInstance.ToTaxDetail(taxDetailDto, currency);
        }

        public static Discount ToDiscount(this orderDto.Discount discountDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            return OrderConverterInstance.ToDiscount(discountDto, availCurrencies, language);
        }

        public static PaymentMethod ToPaymentMethod(this storeDto.PaymentMethod dto, CustomerOrder order)
        {
            return OrderConverterInstance.ToPaymentMethod(dto, order);
        }

        public static PaymentIn ToOrderInPayment(this orderDto.PaymentIn paymentInDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            return OrderConverterInstance.ToOrderInPayment(paymentInDto, availCurrencies, language);
        }

        public static orderDto.PaymentIn ToOrderPaymentInDto(this PaymentIn paymentIn)
        {
            return OrderConverterInstance.ToOrderPaymentInDto(paymentIn);
        }
        public static orderDto.BankCardInfo ToBankCardInfoDto(this BankCardInfo model)
        {
            return OrderConverterInstance.ToBankCardInfoDto(model);
        }

        public static LineItem ToOrderLineItem(this orderDto.LineItem lineItemDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            return OrderConverterInstance.ToOrderLineItem(lineItemDto, availCurrencies, language);
        }

        public static Model.Order.Shipment ToOrderShipment(this orderDto.Shipment shipmentDto, ICollection<Currency> availCurrencies, Language language)
        {
            return OrderConverterInstance.ToOrderShipment(shipmentDto, availCurrencies, language);
        }

        public static ShipmentItem ToShipmentItem(this orderDto.ShipmentItem shipmentItemDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            return OrderConverterInstance.ToShipmentItem(shipmentItemDto, availCurrencies, language);
        }

        public static ShipmentPackage ToShipmentPackage(this orderDto.ShipmentPackage shipmentPackageDto, IEnumerable<Currency> currencies, Language language)
        {
            return OrderConverterInstance.ToShipmentPackage(shipmentPackageDto, currencies, language);
        }

        public static orderDto.Address ToOrderAddressDto(this Address address)
        {
            return OrderConverterInstance.ToOrderAddressDto(address);
        }

        public static Address ToAddress(this orderDto.Address addressDto)
        {
            return OrderConverterInstance.ToAddress(addressDto);
        }

        public static DynamicProperty ToDynamicProperty(this orderDto.DynamicObjectProperty propertyDto)
        {
            return OrderConverterInstance.ToDynamicProperty(propertyDto);
        }

        public static orderDto.DynamicObjectProperty ToOrderDynamicPropertyDto(this DynamicProperty property)
        {
            return OrderConverterInstance.ToOrderDynamicPropertyDto(property);
        }

        public static orderDto.CustomerOrderSearchCriteria ToSearchCriteriaDto(this OrderSearchCriteria criteria)
        {
            return OrderConverterInstance.ToSearchCriteriaDto(criteria);
        }
    }

    public partial class OrderConverter
    {
        public virtual orderDto.CustomerOrderSearchCriteria ToSearchCriteriaDto(OrderSearchCriteria criteria)
        {
            var result = new orderDto.CustomerOrderSearchCriteria();
            result.CustomerId = criteria.CustomerId;
      
            result.Skip = criteria.Start;
            result.Take = criteria.PageSize;
            result.Sort = criteria.Sort;

            return result;
        }

        public virtual DynamicProperty ToDynamicProperty(orderDto.DynamicObjectProperty propertyDto)
        {
            return propertyDto.JsonConvert<coreDto.DynamicObjectProperty>().ToDynamicProperty();
        }

        public virtual orderDto.DynamicObjectProperty ToOrderDynamicPropertyDto(DynamicProperty property)
        {
            return property.ToDynamicPropertyDto().JsonConvert<orderDto.DynamicObjectProperty>();
        }

        public virtual orderDto.Address ToOrderAddressDto(Address address)
        {
            return address.ToCoreAddressDto().JsonConvert<orderDto.Address>();
        }

        public virtual Address ToAddress(orderDto.Address addressDto)
        {
            return addressDto.JsonConvert<coreDto.Address>().ToAddress();
        }

        public virtual ShipmentPackage ToShipmentPackage(orderDto.ShipmentPackage shipmentPackageDto, IEnumerable<Currency> currencies, Language language)
        {
            var result = new ShipmentPackage();

            result.BarCode = shipmentPackageDto.BarCode;
            result.Id = shipmentPackageDto.Id;

            result.Height = shipmentPackageDto.Height;
            result.Weight = shipmentPackageDto.Weight;
            result.Length = shipmentPackageDto.Length;
            result.Width = shipmentPackageDto.Width;
            result.WeightUnit = shipmentPackageDto.WeightUnit;
            result.MeasureUnit = shipmentPackageDto.MeasureUnit;

            if (shipmentPackageDto.Items != null)
            {
                result.Items = shipmentPackageDto.Items.Select(i => ToShipmentItem(i, currencies, language)).ToList();
            }

            return result;
        }

        public virtual ShipmentItem ToShipmentItem(orderDto.ShipmentItem shipmentItemDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            var result = new ShipmentItem();

            result.BarCode = shipmentItemDto.BarCode;            
            result.Id = shipmentItemDto.Id;
            result.LineItemId = shipmentItemDto.LineItemId;
            result.Quantity = shipmentItemDto.Quantity;            

            if (shipmentItemDto.LineItem != null)
            {
                result.LineItem = ToOrderLineItem(shipmentItemDto.LineItem, availCurrencies, language);
            }

            return result;
        }

        public virtual Shipment ToOrderShipment(orderDto.Shipment shipmentDto, ICollection<Currency> availCurrencies, Language language)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(shipmentDto.Currency)) ?? new Currency(language, shipmentDto.Currency);
            var result = new Shipment(currency)
            {
                CancelledDate = shipmentDto.CancelledDate,
                CancelReason = shipmentDto.CancelReason,
                Comment = shipmentDto.Comment,
                EmployeeId = shipmentDto.EmployeeId,
                EmployeeName = shipmentDto.EmployeeName,
                FulfillmentCenterId = shipmentDto.FulfillmentCenterId,
                FulfillmentCenterName = shipmentDto.FulfillmentCenterName,
                Height = shipmentDto.Height,
                Id = shipmentDto.Id,
                Length = shipmentDto.Length,
                MeasureUnit = shipmentDto.MeasureUnit,
                Number = shipmentDto.Number,
                OperationType = shipmentDto.OperationType,
                OrganizationId = shipmentDto.OrganizationId,
                OrganizationName = shipmentDto.OrganizationName,
                ShipmentMethodCode = shipmentDto.ShipmentMethodCode,
                ShipmentMethodOption = shipmentDto.ShipmentMethodOption,
                Status = shipmentDto.Status,
                TaxType = shipmentDto.TaxType,
                Weight = shipmentDto.Weight,
                WeightUnit = shipmentDto.WeightUnit,
                Width = shipmentDto.Width,
                Currency = currency,
                CreatedBy = shipmentDto.CreatedBy,
                CreatedDate = shipmentDto.CreatedDate,
                ModifiedDate = shipmentDto.ModifiedDate,
                ModifiedBy = shipmentDto.ModifiedBy,
            };

            if (shipmentDto.DeliveryAddress != null)
            {
                result.DeliveryAddress = ToAddress(shipmentDto.DeliveryAddress);
            }

            if (!shipmentDto.Discounts.IsNullOrEmpty())
            {
                result.Discounts.AddRange(shipmentDto.Discounts.Select(x => ToDiscount(x, new[] { currency }, language)));
            }

            if (shipmentDto.DynamicProperties != null)
            {
                result.DynamicProperties = shipmentDto.DynamicProperties.Select(ToDynamicProperty).ToList();
            }

            if (shipmentDto.InPayments != null)
            {
                result.InPayments = shipmentDto.InPayments.Select(p => ToOrderInPayment(p, availCurrencies, language)).ToList();
            }

            if (shipmentDto.Items != null)
            {
                result.Items = shipmentDto.Items.Select(i => ToShipmentItem(i, availCurrencies, language)).ToList();
            }

            if (shipmentDto.Packages != null)
            {
                result.Packages = shipmentDto.Packages.Select(p => ToShipmentPackage(p, availCurrencies, language)).ToList();
            }

            result.Price = new Money(shipmentDto.Price ?? 0, currency);
            result.PriceWithTax = new Money(shipmentDto.PriceWithTax ?? 0, currency);
            result.DiscountAmount = new Money(shipmentDto.DiscountAmount ?? 0, currency);
            result.DiscountAmountWithTax = new Money(shipmentDto.DiscountAmountWithTax ?? 0, currency);
            result.Total = new Money(shipmentDto.Total ?? 0, currency);
            result.TotalWithTax = new Money(shipmentDto.TotalWithTax ?? 0, currency);
            result.TaxTotal = new Money(shipmentDto.TaxTotal ?? 0, currency);
            result.TaxPercentRate = (decimal?)shipmentDto.TaxPercentRate ?? 0m;
            if (shipmentDto.TaxDetails != null)
            {
                result.TaxDetails = shipmentDto.TaxDetails.Select(td => ToTaxDetail(td, currency)).ToList();
            }
            return result;
        }

        public virtual LineItem ToOrderLineItem(orderDto.LineItem lineItemDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(lineItemDto.Currency)) ?? new Currency(language, lineItemDto.Currency);

            var result = new LineItem(currency);
            result.CancelledDate = lineItemDto.CancelledDate;
            result.CancelReason = lineItemDto.CancelReason;
            result.CatalogId = lineItemDto.CatalogId;
            result.CategoryId = lineItemDto.CategoryId;
            result.Height = lineItemDto.Height;
            result.Id = lineItemDto.Id;
            result.ImageUrl = lineItemDto.ImageUrl;
            result.IsCancelled = lineItemDto.IsCancelled;
            result.IsGift = lineItemDto.IsGift;
            result.Length = lineItemDto.Length;
            result.MeasureUnit = lineItemDto.MeasureUnit;
            result.Name = lineItemDto.Name;
            result.ProductId = lineItemDto.ProductId;
            result.Quantity = lineItemDto.Quantity;
            result.ReserveQuantity = lineItemDto.ReserveQuantity;
            result.ShippingMethodCode = lineItemDto.ShippingMethodCode;
            result.Sku = lineItemDto.Sku;
            result.TaxType = lineItemDto.TaxType;
            result.Weight = lineItemDto.Weight;
            result.WeightUnit = lineItemDto.WeightUnit;
            result.Width = lineItemDto.Width;
            result.CreatedBy = lineItemDto.CreatedBy;
            result.CreatedDate = lineItemDto.CreatedDate;
            result.ModifiedDate = lineItemDto.ModifiedDate;
            result.ModifiedBy = lineItemDto.ModifiedBy;


            result.ImageUrl = result.ImageUrl.RemoveLeadingUriScheme();
            result.Currency = currency;
            result.DiscountAmount = new Money(lineItemDto.DiscountAmount ?? 0, currency);

            if (lineItemDto.DynamicProperties != null)
            {
                result.DynamicProperties = lineItemDto.DynamicProperties.Select(ToDynamicProperty).ToList();
            }
            result.ListPrice = new Money(lineItemDto.Price ?? 0, currency);
            result.ListPriceWithTax = new Money(lineItemDto.PriceWithTax ?? 0, currency);
            result.DiscountAmount = new Money(lineItemDto.DiscountAmount ?? 0, currency);
            result.DiscountAmountWithTax = new Money(lineItemDto.DiscountAmountWithTax ?? 0, currency);
            result.PlacedPrice = new Money(lineItemDto.PlacedPrice ?? 0, currency);
            result.PlacedPriceWithTax = new Money(lineItemDto.PlacedPriceWithTax ?? 0, currency);
            result.ExtendedPrice = new Money(lineItemDto.ExtendedPrice ?? 0, currency);
            result.ExtendedPriceWithTax = new Money(lineItemDto.ExtendedPriceWithTax ?? 0, currency);
            result.DiscountTotal = new Money(lineItemDto.DiscountTotal ?? 0, currency);
            result.DiscountTotalWithTax = new Money(lineItemDto.DiscountTotalWithTax ?? 0, currency);
            result.TaxTotal = new Money(lineItemDto.TaxTotal ?? 0, currency);
            result.TaxPercentRate = (decimal?)lineItemDto.TaxPercentRate ?? 0m;
            if (!lineItemDto.Discounts.IsNullOrEmpty())
            {
                result.Discounts.AddRange(lineItemDto.Discounts.Select(x => ToDiscount(x, new[] { currency }, language)));
            }
            if (lineItemDto.TaxDetails != null)
            {
                result.TaxDetails = lineItemDto.TaxDetails.Select(td => ToTaxDetail(td, currency)).ToList();
            }

            return result;
        }

        public virtual PaymentIn ToOrderInPayment(orderDto.PaymentIn paymentIn, IEnumerable<Currency> availCurrencies, Language language)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(paymentIn.Currency)) ?? new Currency(language, paymentIn.Currency);
            var retVal = new PaymentIn(currency);
            
            retVal.CancelledDate = paymentIn.CancelledDate;
            retVal.CancelReason = paymentIn.CancelReason;
            retVal.Comment = paymentIn.Comment;
            retVal.CustomerId = paymentIn.CustomerId;
            retVal.CustomerName = paymentIn.CustomerName;
            retVal.GatewayCode = paymentIn.GatewayCode;
            retVal.Id = paymentIn.Id;
            retVal.IncomingDate = paymentIn.IncomingDate;
            retVal.IsApproved = paymentIn.IsApproved;
            retVal.IsCancelled = paymentIn.IsCancelled;
            retVal.CreatedBy = paymentIn.CreatedBy;
            retVal.CreatedDate = paymentIn.CreatedDate;
            retVal.ModifiedDate = paymentIn.ModifiedDate;
            retVal.ModifiedBy = paymentIn.ModifiedBy;
            retVal.Number = paymentIn.Number;
            retVal.IsCancelled = paymentIn.IsCancelled;
            retVal.Number = paymentIn.Number;
            retVal.OperationType = paymentIn.OperationType;
            retVal.OrganizationId = paymentIn.OrganizationId;
            retVal.OrganizationName = paymentIn.OrganizationName;
            retVal.OuterId = paymentIn.OuterId;
            retVal.ParentOperationId = paymentIn.ParentOperationId;
            retVal.Purpose = paymentIn.Purpose;
            retVal.Status = paymentIn.Status;
           

            if (paymentIn.BillingAddress != null)
            {
                retVal.BillingAddress = paymentIn.BillingAddress.ToAddress();
            }

            if (paymentIn.DynamicProperties != null)
            {
                retVal.DynamicProperties = paymentIn.DynamicProperties.Select(ToDynamicProperty).ToList();
            }

            retVal.Sum = new Money(paymentIn.Sum ?? 0, currency);

            if (paymentIn.PaymentMethod != null)
            {
                retVal.GatewayCode = paymentIn.PaymentMethod.Code;
            }
            return retVal;
        }

        public virtual orderDto.PaymentIn ToOrderPaymentInDto(PaymentIn payment)
        {
            var retVal = new orderDto.PaymentIn();
            retVal.CancelledDate = payment.CancelledDate;
            retVal.CancelReason = payment.CancelReason;
            retVal.Comment = payment.Comment;
            retVal.CustomerId = payment.CustomerId;
            retVal.CustomerName = payment.CustomerName;
            retVal.GatewayCode = payment.GatewayCode;
            retVal.Id = payment.Id;
            retVal.IncomingDate = payment.IncomingDate;
            retVal.IsApproved = payment.IsApproved;
            retVal.IsCancelled = payment.IsCancelled;
            retVal.CreatedBy = payment.CreatedBy;
            retVal.CreatedDate = payment.CreatedDate;
            retVal.ModifiedDate = payment.ModifiedDate;
            retVal.ModifiedBy = payment.ModifiedBy;
            retVal.Number = payment.Number;
            retVal.IsCancelled = payment.IsCancelled;
            retVal.Number = payment.Number;
            retVal.OperationType = payment.OperationType;
            retVal.OrganizationId = payment.OrganizationId;
            retVal.OrganizationName = payment.OrganizationName;
            retVal.OuterId = payment.OuterId;
            retVal.ParentOperationId = payment.ParentOperationId;
            retVal.Purpose = payment.Purpose;
            retVal.Status = payment.Status;

            retVal.Sum = (double)payment.Sum.Amount;
            retVal.Currency = payment.Currency.Code;
            retVal.PaymentStatus = payment.Status;

            if (payment.BillingAddress != null)
            {
                retVal.BillingAddress = payment.BillingAddress.ToOrderAddressDto();
            }

            if (payment.DynamicProperties != null)
            {
                retVal.DynamicProperties = payment.DynamicProperties.Select(ToOrderDynamicPropertyDto).ToList();
            }

            //if (payment.GatewayCode != null)
            //{
            //    var a = retVal.GatewayCode;
            //}

            return retVal;
        }
        public virtual orderDto.BankCardInfo ToBankCardInfoDto(BankCardInfo model)
        {
            orderDto.BankCardInfo retVal = null;
            if (model != null)
            {
                retVal = new orderDto.BankCardInfo();
                retVal.BankCardCVV2 = model.BankCardCVV2;
                retVal.BankCardMonth = model.BankCardMonth;
                retVal.BankCardNumber = model.BankCardNumber;
                retVal.BankCardType = model.BankCardType;
                retVal.BankCardYear = model.BankCardYear;
                retVal.CardholderName = model.CardholderName;
            }

            return retVal;
        }

        public virtual Discount ToDiscount(orderDto.Discount discountDto, IEnumerable<Currency> availCurrencies, Language language)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(discountDto.Currency)) ?? new Currency(language, discountDto.Currency);
            var result = new Discount(currency);

            result.Coupon = discountDto.Coupon;
            result.Description = discountDto.Description;
            result.PromotionId = discountDto.PromotionId;

            result.Amount = new Money(discountDto.DiscountAmount ?? 0, currency);

            return result;
        }

        public virtual CustomerOrder ToCustomerOrder(orderDto.CustomerOrder order, ICollection<Currency> availCurrencies, Language language)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(order.Currency)) ?? new Currency(language, order.Currency);

            var result = new CustomerOrder(currency);

            result.CancelledDate = order.CancelledDate;
            result.CancelReason = order.CancelReason;
            result.ChannelId = order.ChannelId;
            result.Comment = order.Comment;
            result.CreatedBy = order.CreatedBy;
            result.CreatedDate = order.CreatedDate;
            result.CustomerId = order.CustomerId;
            result.CustomerName = order.CustomerName;
            result.EmployeeId = order.EmployeeId;
            result.EmployeeName = order.EmployeeName;
            result.Id = order.Id;
            result.IsApproved = order.IsApproved;
            result.IsCancelled = order.IsCancelled;
            result.ModifiedBy = order.ModifiedBy;
            result.ModifiedDate = order.ModifiedDate;
            result.Number = order.Number;
            result.OrganizationId = order.OrganizationId;
            result.OrganizationName = order.OrganizationName;
            result.Status = order.Status;
            result.StoreId = order.StoreId;
            result.StoreName = order.StoreName;
            result.SubscriptionNumber = order.SubscriptionNumber;
           

            if (order.Addresses != null)
            {
                result.Addresses = order.Addresses.Select(ToAddress).ToList();
            }


            if (order.DynamicProperties != null)
            {
                result.DynamicProperties = order.DynamicProperties.Select(ToDynamicProperty).ToList();
            }

            if (order.InPayments != null)
            {
                result.InPayments = order.InPayments.Select(p => ToOrderInPayment(p, availCurrencies, language)).ToList();
            }

            if (order.Items != null)
            {
                result.Items = order.Items.Select(i => ToOrderLineItem(i, availCurrencies, language)).ToList();
            }

            if (order.Shipments != null)
            {
                result.Shipments = order.Shipments.Select(s => ToOrderShipment(s, availCurrencies, language)).ToList();
            }

            if (!order.Discounts.IsNullOrEmpty())
            {
                result.Discounts.AddRange(order.Discounts.Select(x => ToDiscount(x, new[] { currency }, language)));
            }
            if (order.TaxDetails != null)
            {
                result.TaxDetails = order.TaxDetails.Select(td => ToTaxDetail(td, currency)).ToList();
            }
            result.DiscountAmount = new Money(order.DiscountAmount ?? 0, currency);
            result.DiscountTotal = new Money(order.DiscountTotal ?? 0, currency);
            result.DiscountTotalWithTax = new Money(order.DiscountTotalWithTax ?? 0, currency);

            result.PaymentTotal = new Money(order.PaymentTotal ?? 0, currency);
            result.PaymentTotalWithTax = new Money(order.PaymentTotalWithTax ?? 0, currency);
            result.PaymentDiscountTotal = new Money(order.PaymentDiscountTotal ?? 0, currency);
            result.PaymentDiscountTotalWithTax = new Money(order.PaymentDiscountTotalWithTax ?? 0, currency);
            result.PaymentTaxTotal = new Money(order.PaymentTaxTotal ?? 0, currency);
            result.PaymentPrice = new Money(order.PaymentSubTotal ?? 0, currency);
            result.PaymentPriceWithTax = new Money(order.PaymentSubTotalWithTax ?? 0, currency);

            result.Total = new Money(order.Total ?? 0, currency);
            result.SubTotal = new Money(order.SubTotal ?? 0, currency);
            result.SubTotalWithTax = new Money(order.SubTotalWithTax ?? 0, currency);
            result.TaxTotal = new Money(order.TaxTotal ?? 0, currency);

            result.ShippingTotal = new Money(order.ShippingTotal ?? 0, currency);
            result.ShippingTotalWithTax = new Money(order.ShippingTotalWithTax ?? 0, currency);
            result.ShippingTaxTotal = new Money(order.ShippingTaxTotal ?? 0, currency);
            result.ShippingPrice = new Money(order.ShippingSubTotal ?? 0, currency);
            result.ShippingPriceWithTax = new Money(order.ShippingSubTotalWithTax ?? 0, currency);

            result.SubTotalTaxTotal = new Money(order.SubTotalTaxTotal ?? 0, currency);
            result.SubTotalDiscount = new Money(order.SubTotalDiscount ?? 0, currency);
            result.SubTotalDiscountWithTax = new Money(order.SubTotalDiscountWithTax ?? 0, currency);


            return result;
        }

        public virtual TaxDetail ToTaxDetail(orderDto.TaxDetail taxDetailDto, Currency currency)
        {
            var result = new TaxDetail(currency);
            result.Name = taxDetailDto.Name;
            result.Amount = new Money(taxDetailDto.Amount ?? 0, currency);
            result.Rate = new Money(taxDetailDto.Rate ?? 0, currency);
            return result;
        }

        public virtual PaymentMethod ToPaymentMethod(storeDto.PaymentMethod paymentMethodDto, CustomerOrder order)
        {
            var retVal = new PaymentMethod(order.Currency);

            retVal.Code = paymentMethodDto.Code;
            retVal.Description = paymentMethodDto.Currency;
            retVal.IsAvailableForPartial = paymentMethodDto.IsAvailableForPartial ?? false;
            retVal.LogoUrl = paymentMethodDto.LogoUrl;
            retVal.Name = paymentMethodDto.Name;
            retVal.PaymentMethodGroupType = paymentMethodDto.PaymentMethodGroupType;
            retVal.PaymentMethodType = paymentMethodDto.PaymentMethodType;
            retVal.TaxType = paymentMethodDto.TaxType;

            retVal.Priority = paymentMethodDto.Priority ?? 0;

            if (paymentMethodDto.Settings != null)
            {
                retVal.Settings = paymentMethodDto.Settings.Where(x => !x.ValueType.EqualsInvariant("SecureString")).Select(x => x.JsonConvert<platformDto.Setting>().ToSettingEntry()).ToList();
            }

            retVal.Currency = order.Currency;
            retVal.Price = new Money(paymentMethodDto.Price ?? 0, order.Currency);
            retVal.DiscountAmount = new Money(paymentMethodDto.DiscountAmount ?? 0, order.Currency);
            retVal.TaxPercentRate = (decimal?)paymentMethodDto.TaxPercentRate ?? 0m;

            if (paymentMethodDto.TaxDetails != null)
            {
                retVal.TaxDetails = paymentMethodDto.TaxDetails.Select(td => ToTaxDetail(td, order.Currency)).ToList();
            }

            return retVal;
        }

        public virtual TaxDetail ToTaxDetail(storeDto.TaxDetail dto, Currency currency)
        {
            var result = new TaxDetail(currency);
            result.Amount = new Money(dto.Amount ?? 0, currency);
            result.Rate = new Money(dto.Rate ?? 0, currency);
            result.Name = dto.Name;
            return result;
        }
    }
}