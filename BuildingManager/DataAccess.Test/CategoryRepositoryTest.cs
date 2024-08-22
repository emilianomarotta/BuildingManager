using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class CategoryRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private CategoryRepository _categoryRepository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var contextOptions = new DbContextOptionsBuilder<BuildingManagerContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new BuildingManagerContext(contextOptions);
            _context.Database.EnsureCreated();

            _categoryRepository = new CategoryRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllCategories()
        {
            var expectedCategories = TestData();
            LoadContext(expectedCategories);

            var retrievedCategories = _categoryRepository.GetAll<Category>();
            CollectionAssert.AreEqual(expectedCategories, retrievedCategories.ToList());
        }

        [TestMethod]
        public void GetAllCategoriesByFilter()
        {
            var expectedCategories = TestData();
            LoadContext(expectedCategories);

            var retrievedCategories = _categoryRepository.GetAll<Category>(category => category.Id < 10);
            CollectionAssert.AreEqual(expectedCategories, retrievedCategories.ToList());
        }

        [TestMethod]
        public void GetCategoryById()
        {
            var categories = TestData();
            LoadContext(categories);

            var expectedCategory = new Category { Id = 1, Name = "FirstCategoryTest" };
            var categoryFromDataBase = _categoryRepository.Get(category => category.Id == expectedCategory.Id);

            Assert.AreEqual(expectedCategory, categoryFromDataBase);
        }

        [TestMethod]
        public void InsertCategoryToDataBase()
        {
            var newCategory = new Category { Id = 1, Name = "CategoryTest" };
            List <Category> expectedCategories = new List<Category> { newCategory };
            _categoryRepository.Insert(newCategory);
            var retrievedCategories = _categoryRepository.GetAll<Category>();
            
            CollectionAssert.AreEqual(expectedCategories, retrievedCategories.ToList());
        }

        [TestMethod]
        public void UpdateCategoryNameFromDataBase()
        {
            var categories = TestData();
            LoadContext(categories);

            string newCategoryName = "UpdatedName";
            var categoryToUpdate = _categoryRepository.Get(category => category.Id == 1);
            categoryToUpdate.Name = newCategoryName;

            _categoryRepository.Update(categoryToUpdate);

            var updatedCategory = _categoryRepository.Get(category => category.Id == 1);

            Assert.AreEqual(newCategoryName, updatedCategory.Name);
        }

        [TestMethod]
        public void DeleteCategoryFromDataBase()
        {
            var categoryToDelete = new Category { Id = 1, Name = "CategoryTest" };
            _categoryRepository.Insert(categoryToDelete);

            _categoryRepository.Delete(categoryToDelete);
            var categories = _categoryRepository.GetAll<Category>().ToList();

            CollectionAssert.DoesNotContain(categories, categoryToDelete);
        }

        private void LoadContext(List<Category> categories)
        {
            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }

        private List<Category> TestData()
        {
            return new List<Category>
        {
            new Category { Id = 1, Name = "FirstCategoryTest" },
            new Category { Id = 2, Name = "SecondCategoryTest" }
        };
        }
    }
}