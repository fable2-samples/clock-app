module Model

type Time = int list

type Message = 
    | Tick
    | Reset
    | Start

type Status =
    | Initial 
    | Ticking   

let statToStr = function
    | Initial -> "Initial"
    | Ticking -> "Ticking"
    
let msgToStr = function
    | Tick -> "Tick"
    | Reset -> "Reset"
    | Start -> "Start"

type Model = { Status: Status; Time : Time; }