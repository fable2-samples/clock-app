module FableApp

open Fable.Core.JsInterop
open Fable.Import.Browser
open Model
open Core

let theTimer = document.querySelector("#timer") :?> HTMLElement
let resetButton = document.querySelector("#reset") :?> HTMLButtonElement
let startButton = document.querySelector("#start") :?> HTMLButtonElement

let model = 
    { Status = Initial; Time =[0;0;0;0] } : Model

let viewTime (timer : Time) =
    let leadingZero section =
        if (section <= 9) then
            "0" + section.ToString()
        else
            section.ToString()
    let currentTime = leadingZero(timer.[0]) + ":" + leadingZero(timer.[1]) + ":" + leadingZero(timer.[2]);
    theTimer.innerHTML <- currentTime;

let stopTimer () =
    printf "stop timer"
    window.clearInterval !!(window?myInterval)
    window?myInterval <- null

// default: 1 second tick interval
let tickInterval = 1000

let view { Status = status; Time = time} (dispatcher: MailboxProcessor<Message>) =
    match status with
    | Initial ->
        printf "view: Initial"
        theTimer.innerHTML <- "00:00:00"
        stopTimer()
    | Ticking ->
        printf "view: Ticking"
        if !!(window?myInterval) |> isNull then
            let interval = window.setInterval  ((fun () -> dispatcher.Post Tick), tickInterval, [])
            window?myInterval <- interval
    viewTime time



#nowarn "40"
let rec dispatcher = MailboxProcessor<Message>.Start(fun inbox->    
    // the message processing function
    let rec messageLoop (model : Model) = async{
        // read a message
        let! msg = inbox.Receive()
        printf "inbox: received msg %s" (msgToStr msg)
        printf "update model"
        // process a message
        let newModel = update model msg

        printf "new model:"
        printf " - Status %s" (statToStr model.Status)
        printf " - Time %A" model.Time
        printf "render view using new model"        

        view newModel dispatcher
        // loop to top
        return! messageLoop newModel}

    // start the loop
    messageLoop model)

// press any key: Start 
document.addEventListener_keypress (fun _ -> dispatcher.Post (Start) |> ignore)
// click on start button: Start
startButton.addEventListener_click (fun _ -> dispatcher.Post (Start) |> ignore)
// click on reset button: Reset
resetButton.addEventListener_click (fun _ -> dispatcher.Post (Reset) |> ignore)
