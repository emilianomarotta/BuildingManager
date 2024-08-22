using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;

namespace TestBusinessLogic;

[TestClass]
public class OwnerLogicTest
{
    private Mock<IGenericRepository<Owner>> _ownerRepository;
    private OwnerLogic? _ownerLogic;
    private Owner owner;

    [TestInitialize]
    public void Setup()
    { 
        _ownerRepository = new Mock<IGenericRepository<Owner>>(MockBehavior.Strict);
        _ownerLogic = new OwnerLogic(_ownerRepository.Object);
        owner = new Owner
        {
            Id = 1,
            Email = "owner@owner.com",
            Name = "Owner",
            LastName = "Owner"
        };
    }

    
    [TestMethod]
    public void CreateOwner()
    {
        List<Owner> owners = new List<Owner>();
        _ownerRepository!.Setup(o => o.Insert(owner!));
        _ownerRepository!.Setup(o=> o.GetAll<Owner>()).Returns(owners);
        var newOwner = _ownerLogic!.Create(owner!);
        
        _ownerRepository.VerifyAll();
        Assert.AreEqual(owner, newOwner);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingOwner()
    {
        List<Owner> owners = new List<Owner> { owner };
        _ownerRepository!.Setup(o => o.GetAll<Owner>()).Returns(owners);
        
        Owner ownerCopy = new Owner
        {
            Id = 2,
            Email = "owner@owner.com",
            Name = "Owner",
            LastName = "Owner"
        };
        
        var newOwnerCopy = _ownerLogic!.Create(ownerCopy!);
    }

    [TestMethod]
    public void GetAllOwners()
    {
        List<Owner> owners = new List<Owner> { owner };
        _ownerRepository!.Setup(o => o.GetAll<Owner>()).Returns(owners);
        List<Owner> actualOwners = _ownerLogic.GetAll();
        
        _ownerRepository.VerifyAll();
        CollectionAssert.AreEqual(owners, actualOwners);
    }

    [TestMethod]
    public void GetOwnerById()
    {
        int ownerId = 1;
        List<Owner> owners = new List<Owner> { owner };
        _ownerRepository!.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns(owner!);
        Owner returnedOwner = _ownerLogic.GetById(ownerId);
        
        _ownerRepository.VerifyAll();
        Assert.AreEqual(owner, returnedOwner);
    }

    [TestMethod]
    public void GetNonExistingOwnerById()
    {
        int ownerId = 1;
        _ownerRepository!.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns((Owner)null);
        Owner returnedOwner = _ownerLogic.GetById(ownerId);
        
        _ownerRepository.VerifyAll();
        Assert.IsNull(returnedOwner);
    }

    [TestMethod]
    public void UpdateOwnerInformation()
    {
        int ownerId = 1;
        Owner ownerCopy = new Owner
        {
            Id = 1,
            Email = "owner@owner.com",
            Name = "ownerName",
            LastName = "ownerLastname"
        };
        
        _ownerRepository.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns(owner);
        _ownerRepository.Setup(o => o.Update(owner));
    
        Owner updatedOwner = _ownerLogic.Update(ownerId, ownerCopy);
        
        _ownerRepository.VerifyAll();
        Assert.AreEqual(ownerCopy, updatedOwner);
    }
    
    [TestMethod]
    public void UpdateOwnerEmailToDifferentOne()
    {
        int ownerId = 1;
        Owner ownerCopy = new Owner
        {
            Id = 1,
            Email = "owner@email.com",
            Name = "owner",
            LastName = "owner"
        };
        List<Owner> owners = new List<Owner> { };
        _ownerRepository.Setup(o => o.GetAll<Owner>()).Returns(owners);
        _ownerRepository.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns(owner);
        _ownerRepository.Setup(o => o.Update(owner));
    
        Owner updatedOwner = _ownerLogic.Update(ownerId, ownerCopy);
        
        _ownerRepository.VerifyAll();
        Assert.AreEqual(ownerCopy, updatedOwner);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateOwnerEmailToAlreadyExistingOne()
    {
        int ownerId = 2;
        Owner ownerCopy = new Owner
        {
            Id = 2,
            Email = "email@email.com",
            Name = "owner",
            LastName = "owner"
        };
        
        Owner sameEmailOwner = new Owner
        {
            Id = 2,
            Email = "owner@owner.com",
            Name = "owner",
            LastName = "owner"
        };
        
        List<Owner> owners = new List<Owner> { owner, ownerCopy };
        _ownerRepository.Setup(o => o.GetAll<Owner>()).Returns(owners);
        _ownerRepository.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns(ownerCopy);
        _ownerRepository.Setup(o => o.Update(ownerCopy));
    
        Owner updatedOwner = _ownerLogic.Update(ownerId, sameEmailOwner);
    }
    
    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNonExistingOwner()
    {
        int ownerId = 1;
        Owner ownerCopy = new Owner
        {
            Id = 1,
            Email = "email@email.com",
            Name = "owner",
            LastName = "owner"
        };
        
        _ownerRepository.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns((Owner)null);
        _ownerRepository.Setup(o => o.Update(owner));
    
        Owner updatedOwner = _ownerLogic.Update(ownerId, ownerCopy);
    }

    [TestMethod]
    public void DeleteExistingOwner()
    {
        int ownerId = 1;
        _ownerRepository.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns(owner);
        _ownerRepository.Setup(o => o.Delete(owner));
    
        bool ownerDeleted = _ownerLogic.Delete(ownerId);
        
        _ownerRepository.VerifyAll();
        Assert.IsTrue(ownerDeleted);
    }
    
    [TestMethod]
    public void DeleteNonExistingOwner()
    {
        int ownerId = 1;
        _ownerRepository.Setup(o => o.Get(o => o.Id == ownerId, null)).Returns((Owner)null);
    
        bool ownerDeleted = _ownerLogic.Delete(ownerId);
        
        _ownerRepository.VerifyAll();
        Assert.IsFalse(ownerDeleted);
    }
}