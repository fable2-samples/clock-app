module FableApp

open Fable.Core.JsInterop
open Fable.Import.Browser
open Model
open Core

let theTimer = document.querySelector(".timer") :?> HTMLElement
let resetButton = document.querySelector("#reset") :?> HTMLButtonElement
let startButton = document.querySelector("#start") :?> HTMLButtonElement

let model = 
    { Status = Initial; Time =[0;0;0;0] } : TypingModel
let viewTime (timer : Time) =
    let leadingZero section =
        if (section <= 9) then
            "0" + section.ToString()
        else
            section.ToString()
    let currentTime = leadingZero(timer.[0]) + ":" + leadingZero(timer.[1]) + ":" + leadingZero(timer.[2]);
    theTimer.innerHTML <- currentTime;

let stopTimer () =
    window.clearInterval !!(window?myInterval)
    window?myInterval <- null

let view { Status = status; Time = time} (dispatcher: MailboxProcessor<Message>) =
    match status with
    | Initial ->
        theTimer.innerHTML <- "00:00:00"
        stopTimer()
    | JustStarted ->
        if !!(window?myInterval) |> isNull then
            let interval = window.setInterval  ((fun () -> dispatcher.Post Tick), 10, [])
            window?myInterval <- interval
    | _ -> 
        viewTime time



#nowarn "40"
let rec dispatcher = MailboxProcessor<Message>.Start(fun inbox->

    // the message processing function
    let rec messageLoop (model : TypingModel) = async{
        // read a message
        let! msg = inbox.Receive()
        // process a message
        let newModel = update model msg
        view newModel dispatcher
        // loop to top
        return! messageLoop newModel}

    // start the loop
    messageLoop model)

document.addEventListener_keypress (fun _ -> dispatcher.Post (KeyPress) |> ignore)
startButton.addEventListener_click (fun _ -> dispatcher.Post (KeyPress) |> ignore)
resetButton.addEventListener_click (fun _ -> dispatcher.Post (StartOver) |> ignore)
