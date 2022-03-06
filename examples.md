# Examples

## InvertFromMercator

If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection. Notice that some projections don't include the necessary data to completely recreate an equirect map.

`--projection mercator --invert -f ..\..\Tests\Input\earth_mercator.png -o ..\..\Tests\Output\InvertFromMercator.png`

![InvertFromMercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/InvertFromMercator.png)


## ToLatLong



`--projection LatLong -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToLatLong.png`

![ToLatLong](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToLatLong.png)


## ToEquirect



`--projection Equirect -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEquirect.png`

![ToEquirect](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEquirect.png)


## ToEquirectangular



`--projection Equirectangular -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEquirectangular.png`

![ToEquirectangular](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEquirectangular.png)


## ToEqualArea



`--projection EqualArea -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEqualArea.png`

![ToEqualArea](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEqualArea.png)


## ToSinusoidal



`--projection Sinusoidal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal.png`

![ToSinusoidal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToSinusoidal.png)


## ToSinusoidal2



`--projection Sinusoidal2 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal2.png`

![ToSinusoidal2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToSinusoidal2.png)


## ToMollweide



`--projection Mollweide -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToMollweide.png`

![ToMollweide](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToMollweide.png)


## ToMercator



`--projection Mercator -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToMercator.png`

![ToMercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToMercator.png)


## ToCylindrical



`--projection Cylindrical -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToCylindrical.png`

![ToCylindrical](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToCylindrical.png)


## ToAzimuthal



`--projection Azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToAzimuthal.png`

![ToAzimuthal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToAzimuthal.png)


## ToOrthographic



`--projection Orthographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToOrthographic.png`

![ToOrthographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToOrthographic.png)


## ToRectilinear



`--projection Rectilinear -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToRectilinear.png`

![ToRectilinear](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToRectilinear.png)


## ToStereographic



`--projection Stereographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToStereographic.png`

![ToStereographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToStereographic.png)


## ToGnomonic



`--projection Gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToGnomonic.png`

![ToGnomonic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToGnomonic.png)


## ToPerspective



`--projection Perspective -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToPerspective.png`

![ToPerspective](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToPerspective.png)


## ToBonne



`--projection Bonne -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToBonne.png`

![ToBonne](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToBonne.png)


## ToHammer



`--projection Hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToHammer.png`

![ToHammer](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToHammer.png)


## WidgeDatetimeColor



`--widget Datetime --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgeDatetimeColor.png`

![WidgeDatetimeColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgeDatetimeColor.png)


## WidgetAltitudesBasic



`--widget altitudes -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAltitudesBasic.png`

![WidgetAltitudesBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAltitudesBasic.png)


## WidgetAltitudesColor



`--widget Altitudes --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAltitudesColor.png`

![WidgetAltitudesColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAltitudesColor.png)


## WidgetAltitudesPosition



`--widget Altitudes --wlat 45 --wlon 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAltitudesPosition.png`

![WidgetAltitudesPosition](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAltitudesPosition.png)


## WidgetAltitudesWithProjection



`--widget temporaryhours --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAltitudesWithProjection.png`

![WidgetAltitudesWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAltitudesWithProjection.png)


## WidgetAnalemmaBasic



`--widget analemma -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAnalemmaBasic.png`

![WidgetAnalemmaBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAnalemmaBasic.png)


## WidgetAnalemmaColor



`--widget analemma --widgetcolor 0,255,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAnalemmaColor.png`

![WidgetAnalemmaColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAnalemmaColor.png)


## WidgetAnalemmaSpacing



`--widget analemma --gridx 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAnalemmaSpacing.png`

![WidgetAnalemmaSpacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAnalemmaSpacing.png)


## WidgetAnalemmaWithProjection



`--widget analemma --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetAnalemmaWithProjection.png`

![WidgetAnalemmaWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetAnalemmaWithProjection.png)


## WidgetDatelineBasic



`--widget Dateline -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatelineBasic.png`

![WidgetDatelineBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatelineBasic.png)


## WidgetDatelineColor



`--widget Dateline --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatelineColor.png`

![WidgetDatelineColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatelineColor.png)


## WidgetDatelineDay



`--widget Dateline --wday 180 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatelineDay.png`

![WidgetDatelineDay](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatelineDay.png)


## WidgetDatelineWithProjection



`--widget Dateline --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatelineWithProjection.png`

![WidgetDatelineWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatelineWithProjection.png)


## WidgetDatetimeBasic



`--widget Datetime -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatetimeBasic.png`

![WidgetDatetimeBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatetimeBasic.png)


## WidgetDatetimeDay



`--widget Datetime --wday 180 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatetimeDay.png`

![WidgetDatetimeDay](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatetimeDay.png)


## WidgetDatetimeWithProjection



`--widget Datetime --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetDatetimeWithProjection.png`

![WidgetDatetimeWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetDatetimeWithProjection.png)


## WidgetGridBasic



`--widget grid -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetGridBasic.png`

![WidgetGridBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetGridBasic.png)


## WidgetGridColor



`--widget grid --gridcolor 0,255,0 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetGridColor.png`

![WidgetGridColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetGridColor.png)


## WidgetGridSizing



`--widget grid --gridx 15 --gridy 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetGridSizing.png`

![WidgetGridSizing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetGridSizing.png)


## WidgetGridWithProjection



`--widget grid --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetGridWithProjection.png`

![WidgetGridWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetGridWithProjection.png)


## WidgetLocalHoursBasic



`--widget localhours -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetLocalHoursBasic.png`

![WidgetLocalHoursBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetLocalHoursBasic.png)


## WidgetLocalHoursColor



`--widget localhours --widgetcolor 128,128,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetLocalHoursColor.png`

![WidgetLocalHoursColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetLocalHoursColor.png)


## WidgetLocalHoursPosition



`--widget localhours --wlon 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetLocalHoursPosition.png`

![WidgetLocalHoursPosition](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetLocalHoursPosition.png)


## WidgetLocalHoursWithProjection



`--widget temporaryhours --projection orthographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetLocalHoursWithProjection.png`

![WidgetLocalHoursWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetLocalHoursWithProjection.png)


## WidgetTemporaryHoursBasic



`--widget temporaryhours -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTemporaryHoursBasic.png`

![WidgetTemporaryHoursBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTemporaryHoursBasic.png)


## WidgetTemporaryHoursColor



`--widget temporaryhours --widgetcolor 128,128,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTemporaryHoursColor.png`

![WidgetTemporaryHoursColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTemporaryHoursColor.png)


## WidgetTemporaryHoursPosition



`--widget temporaryhours --wlat 60 --wlon 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTemporaryHoursPosition.png`

![WidgetTemporaryHoursPosition](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTemporaryHoursPosition.png)


## WidgetTemporaryHoursWithProjection



`--widget temporaryhours --projection sinusoidal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTemporaryHoursWithProjection.png`

![WidgetTemporaryHoursWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTemporaryHoursWithProjection.png)


## WidgetTropicsBasic



`--widget Tropics -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTropicsBasic.png`

![WidgetTropicsBasic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTropicsBasic.png)


## WidgetTropicsColor



`--widget Tropics --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTropicsColor.png`

![WidgetTropicsColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTropicsColor.png)


## WidgetTropicsWithProjection



`--widget tropics --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetTropicsWithProjection.png`

![WidgetTropicsWithProjection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WidgetTropicsWithProjection.png)


## WithBackgroundColor

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background color

`--bgcolor 255,0,0 --projection perspective -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundColor.png`

![WithBackgroundColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WithBackgroundColor.png)


## WithBackgroundImage

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background image

`--bg ..\..\Tests\Input\background.png --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundImage.png`

![WithBackgroundImage](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WithBackgroundImage.png)


