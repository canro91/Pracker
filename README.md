# Pracker

![](https://img.shields.io/badge/netstandard-2.0-brightgreen.svg) ![](https://github.com/canro91/Pracker/workflows/Build/badge.svg) ![](https://img.shields.io/github/license/canro91/Pracker)

Pracker updates entities and keeps track of changes. Pracker keeps track of class properties changes from a separate DTO, value object, or view model. Use Pracker to implement poor-man audit logs.

## Usage

### Update and track a single entity

Use Pracker to update a single class property and track what was changed.

```csharp
var user = new User
{
    FirstName = "Before"
};
var userWithTracker = new AuditLogTracker<User>(user);
userWithTracker.UpdateAndTrack(u => u.FirstName, "After");

user.FirstName;
// "After"

userWithTracker.DisplayChanges();
// "Field FirstName, original value: Before, new value: After"
```

### Pass a placeholder for null values

```csharp
var user = new User
{
    FirstName = null
};
var userWithTracker = new AuditLogTracker<User>(user, onNullValue: "***");
userWithTracker.UpdateAndTrack(u => u.FirstName, "After");

user.FirstName;
// "After"

userWithTracker.DisplayChanges();
// "Field FirstName, original value: ***, new value: After"
```

### Only track a single property

Use Pracker to only track the changed value of a property.

```csharp
var user = new User
{
    FirstName = "Before"
};
var userWithTracker = new AuditLogTracker<User>(user);
userWithTracker.Track(u => u.FirstName, "After");

user.FirstName;
// "Before"

userWithTracker.DisplayChanges();
// "Field FirstName, original value: Before, new value: After"
```

### Entity plus separate changes

Use Pracker to track the changes from a separate class: DTO or view model. It will track properties with the same name.

```csharp
var user = new User
{
    FirstName = "BeforeFirstName",
    LastName = "BeforeLastName"
};
var userViewModel = new UserViewModel
{
    FirstName = "AfterFirstName",
    LastName = "AfterLastName"
};
var userWithTracker = new AuditLogTracker<User, UserViewModel>(user, userViewModel);
userWithTracker.TrackAll();

var allChanges = userWithTracker.DisplayChanges();
// "Field FirstName, original value: BeforeFirstName, new value: AfterFirstName"
// "Field LastName, original value: BeforeLastName, new value: AfterLastName"
```

## Installation

Grab your own copy

## Contributing

Feel free to report any bug, ask for a new feature or just send a pull-request. All contributions are welcome.
	
## License

MIT
