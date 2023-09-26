# BookBuddy

BookBuddy is a gamified application designed to train library users and novice librarians in the use of the Dewey Decimal Classification system. Developed for local libraries, this software aims to make the learning experience engaging and fun.

## Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Cloning the Repository](#cloning-the-repository)
  - [Compiling the Software](#compiling-the-software)
  - [Running the Software](#running-the-software)
- [Usage](#usage)
- [Future Enhancements](#future-enhancements)
- [License & Contributing](#license--contributing)

## Features

- **Replacing Books Game**: Users rearrange generated call numbers in ascending order using drag and drop.
- **Gamification Timer Feature**: A timer tracks the user's progress, motivating them to improve their time.
- **Utility Functions**: 
  - Shuffle the current list.
  - Generate a new list of call numbers.
  - Verify the order of the call numbers.

## Prerequisites

- .NET Framework (latest version recommended).
- Windows OS (Windows 10 or newer).

## Getting Started

### Cloning the Repository

To get a copy of the software on your local machine:

git clone https://github.com/johanngeustyn/BookBuddy.git

### Compiling the Software

1. Navigate to the cloned repository's directory.
2. Open `BookBuddy.sln` with Visual Studio.
3. From the top menu, select `Build > Build Solution`.

### Running the Software

1. Set `MainWindow` as the start-up project if it isn't already.
2. Press `F5` or choose `Start Debugging` from the menu.

> Post-build, you can also locate the `BookBuddy.exe` file in the `bin/Debug` or `bin/Release` directory and run it directly.

## Usage

1. **Main Screen**: Presents three options: "Replacing books", "Identifying areas", and "Finding call numbers". Currently, only "Replacing books" is available.
2. **Replacing Books**:
   - Generates 10 random call numbers upon selection. Use drag and drop to rearrange these numbers in ascending order.
   - Use the provided buttons to either shuffle the current list, generate a new list, or verify the order of your list.
   - Aim to beat the clock and improve your time with each attempt!

## Future Enhancements

- **Identifying Areas**: This will test users on identifying which broad area a book belongs to.
- **Finding Call Numbers**: Here, users will have the task of determining the call number for specific topics.

## License & Contributing

This software is developed for educational and training purposes. For guidelines on contributions and feedback, refer to [CONTRIBUTING.md](./CONTRIBUTING.md).
