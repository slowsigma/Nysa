option explicit

Dim x, l, t

x = datepart("s", time)
l = 5

randomize x

function GetMessage(rndValue)
  if (rndValue < 4) then
    GetMessage = "Guten Morgen"
  elseif (rndValue < 6) then
    GetMessage = "Guten Tag"
  else
    GetMessage = "Guten Abend"
  end if
end function

do while (l > 1)
  x = Rnd(x)
  l = l - 1
loop

t = x * 10

MsgBox getmessage(t)
