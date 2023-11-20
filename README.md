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
- [License & Contributing](#license--contributing)

## Features

- **Finding Call Numbers Game**: Users drill deeper and deeper into the hierarchy of call numbers given a question.
- **Identifying Areas Game**: Users match call numbers with their descriptions using drag and drop.
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

1. **Main Screen**: Presents three options: "Replacing books", "Identifying areas", and "Finding call numbers".
2. **Replacing Books**:
   - Generates 10 random call numbers upon selection. Use drag and drop to rearrange these numbers in ascending order.
   - Use the provided buttons to either shuffle the current list, generate a new list, or verify the order of your list.
   - Aim to beat the clock and improve your time with each attempt!
3. **Identifying Areas**:
   - At the start, you're presented with 4 call numbers in one column and 7 descriptions in another. Your task is to correctly match each call number with its corresponding description.
   - Use drag and drop to rearrange the descriptions to align them with the appropriate call numbers.
   - After making your selections, click the submit button to check your matches.
   - Challenge yourself! In the subsequent round, the positions of call numbers and descriptions will swap.
   - Aim for accuracy and speed to master the Dewey Decimal System's top-level classes!
4. **Finding Call Numbers**:
   - When a new game is started, you will be presented with a random third-level call number description.
   - Below the question are 4 options. You're task is to select the right parent category for the given description. At first you will be presented with top-level call numbers, from there you will drill deeper into the second-level parent call numbers until the right answer is selected.
   - Once you have selected your answer, click the submit button to check your answer.
   - If you get a question wrong, the game will restart.
   - Keep track of your score and try to beat your highest score!

## License & Contributing

This software is developed for educational and training purposes. For guidelines on contributions and feedback, refer to [CONTRIBUTING.md](./CONTRIBUTING.md).
