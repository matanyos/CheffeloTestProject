using Work.Database;
using Work.Implementation;

namespace Work.Tests;

public class UserRepositoryTests
{

    [Fact]
    public void Create_ShouldAddUserToDatabase()
    {
        // Arrange
        var mockDatabase = new MockDatabase(2);
        var userRepository = new UserRepository(mockDatabase);

        // Act
        var user = new User { UserId = Guid.NewGuid(), UserName = "Matanyos", Birthday = new DateTime(1993, 11, 9) };
        userRepository.Create(user);

        // Assert
        Assert.Equal(3, mockDatabase.Users.Count);
        Assert.True(mockDatabase.Users.ContainsKey(user.UserId));
    }

    [Fact]
    public void Read_ShouldReturnCorrectUserIfExists()
    {
        // Arrange
        var mockDatabase = new MockDatabase(2);
        var userRepository = new UserRepository(mockDatabase);

        var existingUser = mockDatabase.Users.Values.First();

        // Act
        var result = userRepository.Read(existingUser.UserId);

        // Assert
        Assert.Equal(existingUser, result);
    }

    [Fact]
    public void Update_ShouldUpdateExistingUser()
    {
        // Arrange
        var mockDatabase = new MockDatabase(2);
        var userRepository = new UserRepository(mockDatabase);
        var existingUser = mockDatabase.Users.Values.First();

        var updatedUser = new User { UserId = existingUser.UserId, UserName = "Updated User", Birthday = DateTime.Now.AddYears(-3) };

        // Act
        userRepository.Update(updatedUser);

        // Assert
        Assert.Equal(updatedUser, mockDatabase.Users[existingUser.UserId]);
    }

    [Fact]
    public void Remove_ShouldRemoveUserFromDatabase()
    {
        // Arrange
        var mockDatabase = new MockDatabase(2);
        var userRepository = new UserRepository(mockDatabase);
        var userToRemove = mockDatabase.Users.Values.First();

        // Act
        userRepository.Remove(userToRemove);

        // Assert
        Assert.Single(mockDatabase.Users);
        Assert.DoesNotContain(userToRemove.UserId, mockDatabase.Users.Keys);

    }

    // more can be added like, testing another possible results of the repository methods... 
}