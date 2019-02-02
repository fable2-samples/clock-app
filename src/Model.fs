module Model

type Time = int list

type Message = 
    | Tick
    | StartOver
    | KeyPress

type Status =
    | Initial 
    | JustStarted   
    | Stopped


type TypingModel = { Status: Status; Time : Time; }