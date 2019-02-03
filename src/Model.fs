module Model

type Time = int list

type Message = 
    | Tick
    | Reset
    | Start

type Status =
    | Initial 
    | Ticking   



type Model = { Status: Status; Time : Time; }