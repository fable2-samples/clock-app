module Core

open Model
open System

let updateTime (timer:int list) =
    let t3 =  (timer.[3] + 1) |> float
    let t0 =  (t3/100./60.) |> Math.Floor
    let t1 =  (t3/100. - t0 * 60.) |> Math.Floor
    let t2 =  (t3 - t1 * 100.- t0 * 6000.) |> Math.Floor
    [int t0;int t1;int t2; int t3]



let update (model : TypingModel) = function
    | Tick -> { model with Time = updateTime model.Time}
    | StartOver ->  {model with Status = Initial; Time = [0;0;0;0]; }
    | KeyPress when model.Status = Initial -> { model with Status = JustStarted}


