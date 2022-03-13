# Examples

## Basic Usage

### Invert From Mercator

If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection. Notice that some projections don't include the necessary data to completely recreate an equirect map.

`--projection mercator --invert -w 400 -h 200 -f ..\..\Tests\Input\earth_mercator.png -o ..\..\Tests\Output\InvertFromMercator.png`

![Invert From Mercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/InvertFromMercator.png)

### To Azimuthal

`--projection Azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToAzimuthal.png`

![To Azimuthal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToAzimuthal.png)

### To Bonne

`--projection Bonne -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToBonne.png`

![To Bonne](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToBonne.png)

### To Cylindrical

`--projection Cylindrical -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToCylindrical.png`

![To Cylindrical](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToCylindrical.png)

### To Equal Area

`--projection EqualArea -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEqualArea.png`

![To Equal Area](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToEqualArea.png)

### To Equirect

`--projection Equirect -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEquirect.png`

![To Equirect](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToEquirect.png)

### To Equirectangular

`--projection Equirectangular -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEquirectangular.png`

![To Equirectangular](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToEquirectangular.png)

### To Gnomonic

`--projection Gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToGnomonic.png`

![To Gnomonic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToGnomonic.png)

### To Hammer

`--projection Hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToHammer.png`

![To Hammer](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToHammer.png)

### To Lat Long

`--projection LatLong -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToLatLong.png`

![To Lat Long](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToLatLong.png)

### To Mercator

`--projection Mercator -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToMercator.png`

![To Mercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToMercator.png)

### To Mollweide

`--projection Mollweide -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToMollweide.png`

![To Mollweide](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToMollweide.png)

### To Orthographic

`--projection Orthographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToOrthographic.png`

![To Orthographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToOrthographic.png)

### To Perspective

`--projection Perspective -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToPerspective.png`

![To Perspective](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToPerspective.png)

### To Rectilinear

`--projection Rectilinear -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToRectilinear.png`

![To Rectilinear](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToRectilinear.png)

### To Sinusoidal

`--projection Sinusoidal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal.png`

![To Sinusoidal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToSinusoidal.png)

### To Sinusoidal 2

`--projection Sinusoidal2 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal2.png`

![To Sinusoidal 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToSinusoidal2.png)

### To Stereographic

`--projection Stereographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToStereographic.png`

![To Stereographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ToStereographic.png)

### With Background Color

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background color

`--bgcolor 255,0,0 --projection perspective -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundColor.png`

![With Background Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/WithBackgroundColor.png)

### With Background Image

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background image

`--bg ..\..\Tests\Input\background.png --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundImage.png`

![With Background Image](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/WithBackgroundImage.png)



## Loop Usage

### Loop Basic

Generate a series of images that proceed according to the increment variables you specify. Without increment variables it just outputs the same image over and over.

`--loop 3 --latinc 10 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LoopBasic.png`

![Loop Basic 1](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopBasic0000.png)

![Loop Basic 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopBasic0001.png)

![Loop Basic 3](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopBasic0002.png)

### Loop With Projection

`--projection perspective --loop 6 --latinc 30 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LoopWithProjection.png`

![Loop With Projection 1](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjection0000.png)

![Loop With Projection 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjection0001.png)

![Loop With Projection 3](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjection0002.png)

![Loop With Projection 4](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjection0003.png)

![Loop With Projection 5](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjection0004.png)

![Loop With Projection 6](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjection0005.png)

### Loop With Projection And Params

`--projection orthographic --lat 30 --loop 5 --longinc 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LoopWithProjectionAndParams.png`

![Loop With Projection And Params 1](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjectionAndParams0000.png)

![Loop With Projection And Params 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjectionAndParams0001.png)

![Loop With Projection And Params 3](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjectionAndParams0002.png)

![Loop With Projection And Params 4](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjectionAndParams0003.png)

![Loop With Projection And Params 5](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LoopWithProjectionAndParams0004.png)



## Options Usage

### Adjust

Adjusts the final output size in case your projection needs more room.

`--adjust -w 400 -h 400 --projection mercator -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\Adjust.png`

![Adjust](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/Adjust.png)

### Offset Lat Lon

`--lat 45 --lon 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\OffsetLatLon.png`

![Offset Lat Lon](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/OffsetLatLon.png)

### Offset X

`--xoff 2 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\OffsetX.png`

![Offset X](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/OffsetX.png)

### Offset XY

Applies an offset to the image itself, with units depending on the type of projection. Probably keep it between -2PI and +2PI. Probably not what you want. Included for backward compatibility.

`--xoff 1 --yoff 1 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\OffsetXY.png`

![Offset XY](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/OffsetXY.png)

### Offset Y

`--yoff 1 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\OffsetY.png`

![Offset Y](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/OffsetY.png)

### Radius

`--radius 15 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\Radius.png`

![Radius](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/Radius.png)

### Radius Larger

Note the radius is relative to the projection, not the image, so the shape of the output depends on the projection.

`--radius 75 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\RadiusLarger.png`

![Radius Larger](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/RadiusLarger.png)

### Rotate

`--rotate 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\Rotate.png`

![Rotate](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/Rotate.png)

### Scale Down

`--scale .5 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ScaleDown.png`

![Scale Down](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ScaleDown.png)

### Scale Up

`--scale 2 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ScaleUp.png`

![Scale Up](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/ScaleUp.png)

### Tilt

Like rotating the globe about the equator at 0 longitude.

`--tilt 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\Tilt.png`

![Tilt](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/Tilt.png)

### Turn

Like rotating the globe about its poles.

`--turn 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\Turn.png`

![Turn](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/Turn.png)



## Quality Settings

### Quality Best

`--quality Best --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\QualityBest.png`

![Quality Best](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/QualityBest.png)

### Quality Bicubic

`--quality Bicubic --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\QualityBicubic.png`

![Quality Bicubic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/QualityBicubic.png)

### Quality Bilinear

`--quality Bilinear --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\QualityBilinear.png`

![Quality Bilinear](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/QualityBilinear.png)

### Quality Fast

`--quality Fast --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\QualityFast.png`

![Quality Fast](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/QualityFast.png)

### Quality Good

`--quality Good --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\QualityGood.png`

![Quality Good](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/QualityGood.png)

### Quality Nearest Neighbor

`--quality NearestNeighbor --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\QualityNearestNeighbor.png`

![Quality Nearest Neighbor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/QualityNearestNeighbor.png)



## Widget Usage



### Altitudes Usage

#### Altitudes Basic

`--widget altitudes -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesBasic.png`

![Altitudes Basic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AltitudesBasic.png)

#### Altitudes Color

`--widget Altitudes --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesColor.png`

![Altitudes Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AltitudesColor.png)

#### Altitudes Position

`--widget Altitudes --wlat 45 --wlon 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesPosition.png`

![Altitudes Position](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AltitudesPosition.png)

#### Altitudes With Projection

`--widget temporaryhours --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesWithProjection.png`

![Altitudes With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AltitudesWithProjection.png)



### Analemma Usage

#### Analemma Basic

`--widget analemma -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaBasic.png`

![Analemma Basic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AnalemmaBasic.png)

#### Analemma Color

`--widget analemma --widgetcolor 0,255,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaColor.png`

![Analemma Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AnalemmaColor.png)

#### Analemma Spacing

`--widget analemma --gridx 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaSpacing.png`

![Analemma Spacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AnalemmaSpacing.png)

#### Analemma With Projection

`--widget analemma --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaWithProjection.png`

![Analemma With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/AnalemmaWithProjection.png)



### Dateline Usage

#### Basic Dateline

`--widget Dateline -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicDateline.png`

![Basic Dateline](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicDateline.png)

#### Dateline Color

`--widget Dateline --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatelineColor.png`

![Dateline Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/DatelineColor.png)

#### Dateline Day

`--widget Dateline --wday 180 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatelineDay.png`

![Dateline Day](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/DatelineDay.png)

#### Dateline With Projection

`--widget Dateline --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatelineWithProjection.png`

![Dateline With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/DatelineWithProjection.png)



### Datetime Usage

#### Basic Datetime

`--widget Datetime -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicDatetime.png`

![Basic Datetime](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicDatetime.png)

#### Datetime Color

`--widget Datetime --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatetimeColor.png`

![Datetime Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/DatetimeColor.png)

#### Datetime Day

`--widget Datetime --wday 180 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatetimeDay.png`

![Datetime Day](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/DatetimeDay.png)

#### Datetime With Projection

`--widget Datetime --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatetimeWithProjection.png`

![Datetime With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/DatetimeWithProjection.png)



### Grid Usage

#### Basic Grid

`--widget grid -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicGrid.png`

![Basic Grid](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicGrid.png)

#### Grid Color

`--widget grid --gridcolor 0,255,0 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridColor.png`

![Grid Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/GridColor.png)

#### Grid Sizing

`--widget grid --gridx 15 --gridy 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridSizing.png`

![Grid Sizing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/GridSizing.png)

#### Grid With Projection

`--widget grid --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridWithProjection.png`

![Grid With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/GridWithProjection.png)

#### Grid With Projection 2

`--widget grid --projection perspective --lat 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridWithProjection2.png`

![Grid With Projection 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/GridWithProjection2.png)



### Indicatrix Usage

#### Basic Indicatrix

`--widget Indicatrix -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicIndicatrix.png`

![Basic Indicatrix](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicIndicatrix.png)

#### Indicatrix Color

`--widget Indicatrix --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixColor.png`

![Indicatrix Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/IndicatrixColor.png)

#### Indicatrix Naive Spacing

By default this widget tries to be smart about where it places the indicatrices by skipping some nearer the poles. Use this flag to disable the smartness.

`--widget Indicatrix --wnaivespacing -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixNaiveSpacing.png`

![Indicatrix Naive Spacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/IndicatrixNaiveSpacing.png)

#### Indicatrix Spacing

`--widget Indicatrix --gridx 60 --gridy 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixSpacing.png`

![Indicatrix Spacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/IndicatrixSpacing.png)

#### Indicatrix With Projection

`--widget Indicatrix --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixWithProjection.png`

![Indicatrix With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/IndicatrixWithProjection.png)



### Local Hours Usage

#### Basic Local Hours

`--widget localhours -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicLocalHours.png`

![Basic Local Hours](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicLocalHours.png)

#### Local Hours Color

`--widget localhours --widgetcolor 128,128,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LocalHoursColor.png`

![Local Hours Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LocalHoursColor.png)

#### Local Hours Position

`--widget localhours --wlon 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LocalHoursPosition.png`

![Local Hours Position](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LocalHoursPosition.png)

#### Local Hours With Projection

`--widget temporaryhours --projection orthographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LocalHoursWithProjection.png`

![Local Hours With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/LocalHoursWithProjection.png)



### Temporary Hours Usage

#### Basic Temporary Hours

`--widget temporaryhours -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicTemporaryHours.png`

![Basic Temporary Hours](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicTemporaryHours.png)

#### Temporary Hours Color

`--widget temporaryhours --widgetcolor 128,128,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TemporaryHoursColor.png`

![Temporary Hours Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/TemporaryHoursColor.png)

#### Temporary Hours Position

`--widget temporaryhours --wlat 60 --wlon 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TemporaryHoursPosition.png`

![Temporary Hours Position](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/TemporaryHoursPosition.png)

#### Temporary Hours With Projection

`--widget temporaryhours --projection sinusoidal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TemporaryHoursWithProjection.png`

![Temporary Hours With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/TemporaryHoursWithProjection.png)



### Tropics Usage

#### Basic Tropics

`--widget Tropics -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicTropics.png`

![Basic Tropics](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/BasicTropics.png)

#### Tropics Color

`--widget Tropics --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TropicsColor.png`

![Tropics Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/TropicsColor.png)

#### Tropics With Projection

`--widget tropics --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TropicsWithProjection.png`

![Tropics With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/TropicsWithProjection.png)



### Widget Render Modes

#### Widget Combined Mode

This is the default mode, it returns one image with widgets rendered directly over the map.

`--projection hammer --widget grid --widgetmode combined -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetCombinedMode.png`

![Widget Combined Mode](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/WidgetCombinedMode.png)

#### Widget Only Mode

Does not return the projected map at all, only the projected widgets alone.

`--projection hammer --widget grid --widgetmode widgetonly -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetOnlyMode.png`

![Widget Only Mode](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/WidgetOnlyMode_Widgets.png)

#### Widget Separate Mode

Returns two separate, projected images, one of the map, one of the matching widgets

`--projection hammer --widget grid --widgetmode separate -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WidgetSeparateMode.png`

![Widget Separate Mode 1](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/WidgetSeparateMode.png)

![Widget Separate Mode 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorTests/Tests/Output/WidgetSeparateMode_Widgets.png)

