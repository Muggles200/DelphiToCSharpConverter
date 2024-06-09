program Sample2;

uses
  Classes;

var
  List: TList;

begin
  List := TList.Create;
  try
    List.Add(nil);
  finally
    List.Free;
  end;
end.
