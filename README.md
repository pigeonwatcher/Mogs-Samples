# Mogs: Coffee Craving Cats!
<p align="center" width="100%"><img width="33%" src="https://github.com/pigeonwatcher/Mogs-Samples/blob/main/banner.png"></p>

Mogs is a Tamagotchi-like-cat-petting-cake-eating-collectible game! The player takes care of a fluffy pet cat who likes to invite their pals over for coffee and treats. In order to keep the cat happy, it must be fed regularly, given A LOT of attention, and encouraged to socialise with its pals.

## The Goal

Meet all the pals! The player can keep track of which pals have visited through the cat-alogue *wink* *wink*. Although—as I'm sure we all know—cats have different coffee tastes. Hence, it's important for the player to keep that pot of sweet, sweet go-go juice brewing with all sorts of different flavors: earthy, chocolatey, fruity, nutty, and more! And don't forget to have their favorite treat waiting, as being best pals with these cats comes with some groovy rewards!

<p align="center" width="100%"><img width="10%" src="https://github.com/pigeonwatcher/Mogs-Samples/blob/main/cat.png"></p>

## Contents

* Inventory [[1]](./mogs/services/InventoryService.cs) [[2]](./mogs/gameobjects/other/Items)
* UI [[1]](./mogs/services/UIManagementService.cs) [[2]](./mogs/ui/elements)
* Saving [[1]](./mogs/services/SaveGameService.cs)
* Rendering with Custom Shader [[1]](./mogs/systems/EntityRenderSystem.cs) [[2]](./mogs/Content/catcolors.fx)
* Emoji Animations [[1]](./mogs/systems/EmoteSystem.cs) [[2]](./mogs/gameobjects/other/emotes)
* Shops [[1]](./mogs/services/ShopService.cs)
* Scene Changing [[1]](./mogs/services/SceneManagementService.cs) [[2]](./mogs/scenes)
* Cat Customisation [[1]](./mogs/systems/AccessorySystem.cs) [[2]](./mogs/gameobjects/other/accessories)
* Hunger System [[1]](./mogs/systems/HungerSystem.cs)

## Background

This project was an exciting portfolio piece to pursue, which I'd certainly want to complete and release someday. It was an excellent opportunity to learn and apply some design patterns such as the Factory, Observer, Service Locator, and Finite State Machines (FSM) - definitely recommend reading Robert Nystrom's "Game Programming Patterns"! In fact, solving problems concerning code architecture was certainly the most enlightening lesson I learned from this project (hence, my seemingly sudden desire to read Robert Nystrom).

## Future

In its current state, there are still many areas that can be improved, such as batching draw calls, achieving better separation between business logic and gameplay logic, and enhancing overall code readability, to name a few. However, my focus has now shifted towards a new project 👀. For now, I aim to expand my game portfolio and explore a broader tech stack beyond my coffee-addicted-cat game.

## Other

Many classes from the project have been omitted from this repo, mostly for the sake of brevity. The reasons for not including certain classes are as follows:
* They have functionalities similar to those of a class already showcased.
* Their functionality is simple or straightforward.
* They are mostly incomplete and/or require a major refactor.
