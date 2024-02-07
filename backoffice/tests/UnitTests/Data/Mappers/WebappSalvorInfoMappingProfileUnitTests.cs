using AutoMapper;
using Droits.Data.Mappers;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Data.Mappers
{
    public class WebappSalvorInfoMappingProfileTests
    {
        private readonly IMapper _mapper;
        

public WebappSalvorInfoMappingProfileTests()
{
    var profile = new WebappSalvorInfoMappingProfile();
    var profile2 = new WebappSalvorInfoDroitMappingProfile();
    var profile3 = new WebappSalvorInfoWreckMaterialMappingProfile();
    var configuration = new MapperConfiguration(cfg =>
        {
        cfg.AddProfile(profile);
        cfg.AddProfile(profile2);
        cfg.AddProfile(profile3);
        }
    );

    _mapper = new Mapper(configuration);
}


        [Fact]
        public void TestMapping_SalvorToSalvorInfoDto()
        {
            // Arrange
            var salvor = new Salvor()
            {
                Id = Guid.NewGuid(),
                Name = "Foo",
                Email = "Foo@Bar.com",
                TelephoneNumber = "01234567890",
                Address = new Address()
                {
                    Line1 = "123 Main St",
                    Line2 = "Apt 4B",
                    Town = "Cityville",
                    County = "County",
                    Postcode = "12345"  
                },
                Droits= new List<Droit>(){
                    new()
                    {
                        Reference = "01/24",
                        Status = DroitStatus.Received,
                        RecoveredFrom = RecoveredFrom.Seabed,
                        Latitude = 11,
                        Longitude = 11, 
                        DateFound = new DateTime(2024,1,1),
                        ReportedDate = new DateTime(2024,1,2),
                        LastModified = new DateTime(2024,1,3),
                        WreckMaterials = new List<WreckMaterial>()
                        {
                            new()
                            {
                                Description = "First Wreck Material",
                                Outcome = WreckMaterialOutcome.ReturnedToOwner,
                                StorageAddress = new Address()
                                {
                                    Line1 = "123 Main St",
                                    Line2 = "Apt 4B",
                                    Town = "Cityville",
                                    County = "County",
                                    Postcode = "12345"
                                }
                            },
                            new()
                            {
                                Description = "Second Wreck Material",
                                Outcome = WreckMaterialOutcome.DonatedToMuseum,
                                StorageAddress = new Address()
                                {
                                    Line1 = "123 Main St",
                                    Line2 = "Apt 4B",
                                    Town = "Cityville",
                                    County = "County",
                                    Postcode = "12345"
                                }
                            }
                        }
                    }
                }
                
            };

            // Act
            var salvorInfo = _mapper.Map<SalvorInfoDto>(salvor);

            // Assert
            Assert.Equal(salvor.Id.ToString(), salvorInfo.Id);
            Assert.Equal(salvor.Name, salvorInfo.Name);
            Assert.Equal(salvor.Email, salvorInfo.Email);
            Assert.Equal(salvor.TelephoneNumber, salvorInfo.TelephoneNumber);
            
            Assert.Equal(salvor.Address.Line1, salvorInfo.Address?.Line1);
            Assert.Equal(salvor.Address.Line2, salvorInfo.Address?.Line2);
            Assert.Equal(salvor.Address.Town, salvorInfo.Address?.Town);
            Assert.Equal(salvor.Address.County, salvorInfo.Address?.County);
            Assert.Equal(salvor.Address.Postcode, salvorInfo.Address?.Postcode);
            
            Assert.Equal(salvor.Droits.First().Id.ToString(), salvorInfo.Reports.First().Id);
            Assert.Equal(salvor.Droits.First().Reference, salvorInfo.Reports.First().Reference);
            Assert.Equal(salvor.Droits.First().Status.ToString(), salvorInfo.Reports.First().Status);
            Assert.Equal(salvor.Droits.First().RecoveredFrom.ToString(), salvorInfo.Reports.First().RecoveredFrom);
            Assert.Equal($"{salvor.Droits.First().Latitude}°,{salvor.Droits.First().Longitude}°", salvorInfo.Reports.First().Coordinates);
            Assert.Equal(salvor.Droits.First().DateFound.ToString(), salvorInfo.Reports.First().DateFound);
            Assert.Equal(salvor.Droits.First().ReportedDate.ToString(), salvorInfo.Reports.First().DateReported);
            Assert.Equal(salvor.Droits.First().LastModified.ToString(), salvorInfo.Reports.First().LastUpdated);
            
            Assert.Contains(salvorInfo.Reports.First().WreckMaterials, wm => wm.Description == "First Wreck Material");
            Assert.Contains(salvorInfo.Reports.First().WreckMaterials, wm => wm.Description == "Second Wreck Material");
            Assert.Contains(salvorInfo.Reports.First().WreckMaterials, wm => wm.Outcome == "ReturnedToOwner");
            Assert.Contains(salvorInfo.Reports.First().WreckMaterials, wm => wm.Outcome == "DonatedToMuseum");
            
        }
        //
        //     [Fact]
        //     public void TestMapping_SubmittedReportDtoWithNullValues()
        //     {
        //         // Arrange
        //         var submittedReportDto = new SubmittedReportDto
        //         {
        //             Personal = null
        //         };
        //
        //         // Act
        //         var salvor = _mapper.Map<Salvor>(submittedReportDto);
        //
        //         // Assert
        //         Assert.Equal(string.Empty, salvor.Email);
        //         Assert.Equal(string.Empty, salvor.Name);
        //         Assert.Equal(string.Empty, salvor.TelephoneNumber);
        //         Assert.Equal(string.Empty, salvor.Address.Line1);
        //         Assert.Equal(string.Empty, salvor.Address.Line2);
        //         Assert.Equal(string.Empty, salvor.Address.Town);
        //         Assert.Equal(string.Empty, salvor.Address.County);
        //         Assert.Equal(string.Empty, salvor.Address.Postcode);
        //     }
    }
}
    
