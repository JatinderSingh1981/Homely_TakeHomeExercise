using System;
using Xunit;
using Business.API;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using DBContext.API;
using Common.API;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using Mapper.API;
using Repository.API;
using Microsoft.Extensions.Caching.Memory;
using ViewModels.API;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace UnitTest.API
{


    internal class PropertyListingTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new PropertyListingRequest { Suburb = "South" } };
            yield return new object[] { new PropertyListingRequest { Suburb = "SouthBank", CategoryType = CategoryType.Land, StatusType = StatusType.Current } };
            yield return new object[] { new PropertyListingRequest { Suburb = "SouthBank", CategoryType = CategoryType.Residential, StatusType = StatusType.Deleted } };
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
    internal class CacheTest
    {
        internal PropertyListingRequest ListingRequest { get; set; }
        internal string Message { get; set; }
    }
    internal class CacheTestData : IEnumerable<CacheTest[]>
    {
        public IEnumerator<CacheTest[]> GetEnumerator()
        {
            yield return new CacheTest[] { new CacheTest() { ListingRequest = new PropertyListingRequest { Suburb = "Port Melbourne", CategoryType = CategoryType.Residential, StatusType = StatusType.Current }, Message = "Cache Miss -- Fetched Data From DB" } };
            yield return new CacheTest[] { new CacheTest() { ListingRequest = new PropertyListingRequest { Suburb = "Port Melbourne", CategoryType = CategoryType.Residential, StatusType = StatusType.Current }, Message = "Cache HIT -- Found Data in Cache" } };
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ListingUnitTest
    {
        private readonly IPropertyListingBusiness _propertyListingBusiness;

        public ListingUnitTest()
        {


            AppSettings appSettings = new AppSettings();
            appSettings.ConnectionStrings = new ConnectionStrings() { ApiDatabase = $"Server=JS_HP\\MSSQLSERVER2019;Database=Backend-TakeHomeExercise;Trusted_Connection=True;MultipleActiveResultSets=true" };
            var settingOptions = Options.Create<AppSettings>(appSettings);
            var options = new DbContextOptionsBuilder<DataContext>().Options;

            var dbContext = new DataContext(options, settingOptions);

            var mock = new Mock<ILogger<PropertyListingBusiness>>();
            ILogger<PropertyListingBusiness> bLogger = mock.Object;

            //or use this short equivalent 
            var rLogger = Mock.Of<ILogger<PropertyListingRepository>>();


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PropertyListingMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            //_mapper = mapper;


            var listingRepository = new PropertyListingRepository(dbContext, settingOptions, rLogger);
            var listingResponse = new ViewModels.API.PropertyListingResponse();

            //var memoryCacheOptions = Mock.Of<IOptions<MemoryCacheOptions>>();
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            var cacheManager = new CacheManager(memoryCache);

            _propertyListingBusiness = new PropertyListingBusiness(bLogger, mapper, listingRepository, listingResponse, cacheManager);


        }

        //Check what happens when no values are passed. 
        //Does it use default values and fetch the data?
        //It should fetch the rows equivalent to default rows
        [Fact]
        public async void Test1()
        {
            PropertyListingRequest propertyListingRequest = new PropertyListingRequest();
            var response = await _propertyListingBusiness.GetListing(propertyListingRequest);

            Assert.True(response.Items.Count() > 0 && response.Items.Count() == propertyListingRequest.Take.GetValueOrDefault());
        }

        //Check what happens, if all the parameters (Suburb, CategoryType, StatusType) are passed with the correct values. 
        //Does it fetch the data?
        [Fact]
        public async void Test2()
        {

            PropertyListingRequest propertyListingRequest = new PropertyListingRequest();
            propertyListingRequest.Suburb = "SouthBank";
            propertyListingRequest.CategoryType = CategoryType.Rental;
            propertyListingRequest.StatusType = StatusType.Current;
            propertyListingRequest.Take = 20;
            var response = await _propertyListingBusiness.GetListing(propertyListingRequest);

            Assert.True(response.Items.Count() > 0 && response.Items.Count() == propertyListingRequest.Take.GetValueOrDefault());
        }

        //Check what happens if the parameters with incorrect values such as Suburb ="South" are passed.
        [Theory]
        [ClassData(typeof(PropertyListingTestData))]
        public async void Test3(PropertyListingRequest propertyListingRequest)
        {
            var response = await _propertyListingBusiness.GetListing(propertyListingRequest);
            Assert.True(response.Items == null);
        }

        //Check if data is coming from cache when called next time. 
        //Constructor is getting called n number of times because of which MemoryCache is getting created and reset every time
        //[Theory]
        //[ClassData(typeof(CacheTestData))]
        //public async void Test4(PropertyListingRequest propertyListingRequest, string expected)
        //{
        //    var response = await _propertyListingBusiness.GetListing(propertyListingRequest);
        //    Assert.Equal(response.Message, expected);
        //}

        //Check if data is coming from cache when called next time.
        [Fact]
        public async void Test4()
        {
            var cacheData = new CacheTestData();
            foreach (var item in cacheData.FirstOrDefault())
            {
                var response = await _propertyListingBusiness.GetListing(item.ListingRequest);
                Assert.Equal(response.Message, item.Message);
            }

        }

        //Check what happens for Skip and Take Data Annotations
        [Fact]
        public async Task Test5()
        {
            PropertyListingRequest propertyListingRequest = new PropertyListingRequest();
            propertyListingRequest.Skip = -5;
            propertyListingRequest.Take = 0;
            Assert.Contains(ValidateModel(propertyListingRequest), v => v.MemberNames.Contains("Skip") && v.ErrorMessage.Contains("Value must be greater than 0"));
            Assert.Contains(ValidateModel(propertyListingRequest), v => v.MemberNames.Contains("Take") && v.ErrorMessage.Contains("Value must be between 0 and 500"));

            var nullEx = await Assert.ThrowsAsync<ArgumentNullException>(() => _propertyListingBusiness.GetListing(null));
            Assert.Equal("Value cannot be null. (Parameter 'request')", nullEx.Message);

        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }


}
