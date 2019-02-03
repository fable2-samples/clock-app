# Fable Clock

This project is a simple web app written in [Fable](http://fable.io/).
You can find more templates by searching `Fable.Template` packages in [Nuget](https://www.nuget.org).

This Clock app demonstrates a `event` -> `update` -> `render` loop (simple "elmish like" design)

The state is continually updated using a timer. Each timer tick updates the state, forcing an update of the view.

## Run the app

You can run the project by doing:

Development mode

```bash
yarn install
npx webpack-dev-server
```

Production mode

```bash
yarn install
npx webpack
```

Note: Please have patience and give webpack up to 5 secs to display its progress. It needs to warm up first ;)

Alternatively use:

- `yarn run start:dev` to start webpack dev server (hot reload)
- `yarn run start:prod` for production webpack server (using minified build)

The application compiles and is displayed in the browser.
However, when button is clicked nothing happens (yet). There is a bug in the code which will be fixed shortly. Feel free to help out :)

## Monitor program flow

The program has a number of `printf` statements which call `console.log` to the browser console.

- Open the browser console to monitor and track the internal program flow.
- Add more information to these logs as needed.

## Clock app design

### Domain model

`Model.fs` specifies the domain model.

The `Status` type defined each of the states of the app

- `Initial` (Clock not started/ticking)
- `Ticking` (Clock is started and ticking)

The `Message` type defines the different types of Messages the app can dispatch (and receive).

### Dispatcher

A `dispatcher` is defined using a `MailboxProcessor` which takes incoming `Message`s to be processed.

For each message `msg` received, the `dispatcher` calls `update model msg` to update and create a new (derived) `model`.

`let newModel = update model msg`

The dispatcher then calls `view newModel dispatcher` to render the view using the new `model`.

### Update model

The `update` function (in `Core.fs`) takes the `model` and a `message` and updates the model depending on the type of message received.

- `Tick` calls `updateTime` with the time of the model
- `Reset` sets model Time to initial time: `[0;0;0;0];`
- `Start` sets `Status` to `Ticking` if currently `Status` is `Initial` in order to to make the app start ticking!

The function `updateTime` simply calculates and returns the new time to be displayed (`int list -> int list`)

### View update

The `view` function updates the view depending on the incoming app `Status` that is matched

- `Initial` sets the timer element to `"00:00:00"` stops the current timer by deleting it from window
- `Ticking` if there is no timer set on the `window` it creates a new one

On any view call, it calls `viewTime` with the current `time` to be displayed in the timer element (`#timer`).

## Requirements

- [dotnet SDK](https://www.microsoft.com/net/download/core) 2.0 or higher
- [node.js](https://nodejs.org) 6.11 or higher
- [yarn](https://yarnpkg.com): JS package manager
- [mono](http://www.mono-project.com/) on macOS/Linux to run [paket](https://fsprojects.github.io/Paket/).

## Editor

The project can be used by editors compatible with the new .fsproj format, like VS Code + [Ionide](http://ionide.io/), Emacs with [fsharp-mode](https://github.com/fsharp/emacs-fsharp-mode) or [Rider](https://www.jetbrains.com/rider/). **Visual Studio for Mac** is also compatible but in the current version the package auto-restore function conflicts with Paket so you need to disable it: `Preferences > Nuget > General`.

## Building and running the app

> In the commands below, yarn is the tool of choice. If you want to use npm, just replace `yarn` by `npm` in the commands.

- Install dependencies: `yarn`
- Start Fable daemon and [Webpack](https://webpack.js.org/) dev server: `yarn start`
- In your browser, open: http://localhost:8080/

> Check the `scripts` section in `package.json` for more info. If you are using VS Code + [Ionide](http://ionide.io/), you can also use F5 or Ctrl+Shift+B (Cmd+Shift+B on macOS) instead of typing `yarn start`. With this Fable-specific errors will be highlighted in the editor along with other F# errors. See _Debugging in VS Code_ below.

Any modification you do to the F# code will be reflected in the web page after saving. When you want to output the JS code to disk, run `yarn build` and you'll get your frontend files ready for deployment in the `build` folder.

## Debugging in VS Code

- Install [Debugger For Chrome](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome) in vscode
- Press F5 in vscode
- After all the .fs files are compiled, the browser will be launched
- Set a breakpoint in F#
- Restart with Ctrl+Shift+F5 (Cmd+Shift+F5 on macOS)
- The breakpoint will be caught in vscode

## Project structure

### Paket

[Paket](https://fsprojects.github.io/Paket/) is the package manager used for F# dependencies. It doesn't need a global installation, the binary is included in the `.paket` folder. Other Paket related files are:

- **paket.dependencies**: contains all the dependencies in the repository.
- **paket.references**: there should be one such a file next to each `.fsproj` file.
- **paket.lock**: automatically generated, but should be committed to source control, [see why](https://fsprojects.github.io/Paket/faq.html#Why-should-I-commit-the-lock-file).
- **Nuget.Config**: prevents conflicts with Paket in machines with some Nuget configuration.

> Paket dependencies will be installed in a cache. See [Paket website](https://fsprojects.github.io/Paket/) for more info.

### yarn

- **package.json**: contains the JS dependencies together with other info, like development scripts.
- **yarn.lock**: is the lock file created by yarn.

> JS dependencies will be installed in `node_modules`. See [yarn](https://yarnpkg.com) website for more info.

### Webpack

[Webpack](https://webpack.js.org) is a bundler, which links different JS sources into a single file making deployment much easier. It also offers other features, like a static dev server that can automatically refresh the browser upon changes in your code or a minifier for production release. Fable interacts with Webpack through the `fable-loader`.

There is a single Webpack configuration file `webpack.config.js` are in the root directory. If you need to edit it, check [Webpack](https://webpack.js.org) website for more information.

### F# source files

The template only contains two F# source files: the project (.fsproj) and a source file (.fs) in `src` folder.

## Where to go from here

- [Fable samples](https://github.com/fable2-samples)
- dotnet templates:
  - `Fable.Template.Elmish.React`
  - `SAFE.Template`
- [awesome-fable](https://github.com/kunjee17/awesome-fable#-awesome-fable)
