# Examples

## Basic Usage

### Invert From Mercator

If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection. Notice that some projections don't include the necessary data to completely recreate an equirect map.

`--projection mercator --invert -f ..\..\Tests\Input\earth_mercator.png -o ..\..\Tests\Output\InvertFromMercator.png`

![Invert From Mercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/InvertFromMercator.png)

### To Azimuthal

`--projection Azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToAzimuthal.png`

![To Azimuthal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToAzimuthal.png)

### To Bonne

`--projection Bonne -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToBonne.png`

![To Bonne](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToBonne.png)

### To Cylindrical

`--projection Cylindrical -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToCylindrical.png`

![To Cylindrical](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToCylindrical.png)

### To Equal Area

`--projection EqualArea -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEqualArea.png`

![To Equal Area](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEqualArea.png)

### To Equirect

`--projection Equirect -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEquirect.png`

![To Equirect](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEquirect.png)

### To Equirectangular

`--projection Equirectangular -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToEquirectangular.png`

![To Equirectangular](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEquirectangular.png)

### To Gnomonic

`--projection Gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToGnomonic.png`

![To Gnomonic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToGnomonic.png)

### To Hammer

`--projection Hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToHammer.png`

![To Hammer](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToHammer.png)

### To Lat Long

`--projection LatLong -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToLatLong.png`

![To Lat Long](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToLatLong.png)

### To Mercator

`--projection Mercator -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToMercator.png`

![To Mercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToMercator.png)

### To Mollweide

`--projection Mollweide -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToMollweide.png`

![To Mollweide](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToMollweide.png)

### To Orthographic

`--projection Orthographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToOrthographic.png`

![To Orthographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToOrthographic.png)

### To Perspective

`--projection Perspective -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToPerspective.png`

![To Perspective](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToPerspective.png)

### To Rectilinear

`--projection Rectilinear -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToRectilinear.png`

![To Rectilinear](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToRectilinear.png)

### To Sinusoidal

`--projection Sinusoidal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal.png`

![To Sinusoidal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToSinusoidal.png)

### To Sinusoidal 2

`--projection Sinusoidal2 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal2.png`

![To Sinusoidal 2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToSinusoidal2.png)

### To Stereographic

`--projection Stereographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ToStereographic.png`

![To Stereographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToStereographic.png)

### With Background Color

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background color

`--bgcolor 255,0,0 --projection perspective -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundColor.png`

![With Background Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WithBackgroundColor.png)

### With Background Image

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background image

`--bg ..\..\Tests\Input\background.png --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundImage.png`

![With Background Image](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WithBackgroundImage.png)



## Options Usage

### Radius

`--radius 15 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\Radius.png`

![Radius](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/Radius.png)

### Radius Larger

Note the radius is relative to the projection, not the image, so the shape of the output depends on the projection.

`--radius 75 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\RadiusLarger.png`

![Radius Larger](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/RadiusLarger.png)

### Scale Down

`--scale .5 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ScaleDown.png`

![Scale Down](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ScaleDown.png)

### Scale Up

`--scale 2 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\ScaleUp.png`

![Scale Up](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ScaleUp.png)



## Widget Usage



### Altitudes Usage

#### Altitudes Basic

`--widget altitudes -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesBasic.png`

![Altitudes Basic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AltitudesBasic.png)

#### Altitudes Color

`--widget Altitudes --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesColor.png`

![Altitudes Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AltitudesColor.png)

#### Altitudes Position

`--widget Altitudes --wlat 45 --wlon 45 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesPosition.png`

![Altitudes Position](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AltitudesPosition.png)

#### Altitudes With Projection

`--widget temporaryhours --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AltitudesWithProjection.png`

![Altitudes With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AltitudesWithProjection.png)



### Analemma Usage

#### Analemma Basic

`--widget analemma -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaBasic.png`

![Analemma Basic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AnalemmaBasic.png)

#### Analemma Color

`--widget analemma --widgetcolor 0,255,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaColor.png`

![Analemma Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AnalemmaColor.png)

#### Analemma Spacing

`--widget analemma --gridx 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaSpacing.png`

![Analemma Spacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AnalemmaSpacing.png)

#### Analemma With Projection

`--widget analemma --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\AnalemmaWithProjection.png`

![Analemma With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/AnalemmaWithProjection.png)



### Dateline Usage

#### Basic Dateline

`--widget Dateline -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicDateline.png`

![Basic Dateline](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicDateline.png)

#### Dateline Color

`--widget Dateline --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatelineColor.png`

![Dateline Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/DatelineColor.png)

#### Dateline Day

`--widget Dateline --wday 180 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatelineDay.png`

![Dateline Day](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/DatelineDay.png)

#### Dateline With Projection

`--widget Dateline --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatelineWithProjection.png`

![Dateline With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/DatelineWithProjection.png)



### Datetime Usage

#### Basic Datetime

`--widget Datetime -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicDatetime.png`

![Basic Datetime](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicDatetime.png)

#### Datetime Color

`--widget Datetime --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatetimeColor.png`

![Datetime Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/DatetimeColor.png)

#### Datetime Day

`--widget Datetime --wday 180 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatetimeDay.png`

![Datetime Day](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/DatetimeDay.png)

#### Datetime With Projection

`--widget Datetime --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\DatetimeWithProjection.png`

![Datetime With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/DatetimeWithProjection.png)



### Grid Usage

#### Basic Grid

`--widget grid -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicGrid.png`

![Basic Grid](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicGrid.png)

#### Grid Color

`--widget grid --gridcolor 0,255,0 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridColor.png`

![Grid Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/GridColor.png)

#### Grid Sizing

`--widget grid --gridx 15 --gridy 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridSizing.png`

![Grid Sizing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/GridSizing.png)

#### Grid With Projection

`--widget grid --projection hammer -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\GridWithProjection.png`

![Grid With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/GridWithProjection.png)



### Indicatrix Usage

#### Basic Indicatrix

`--widget Indicatrix -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicIndicatrix.png`

![Basic Indicatrix](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicIndicatrix.png)

#### Indicatrix Color

`--widget Indicatrix --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixColor.png`

![Indicatrix Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/IndicatrixColor.png)

#### Indicatrix Naive Spacing

By default this widget tries to be smart about where it places the indicatrices by skipping some nearer the poles. Use this flag to disable the smartness.

`--widget Indicatrix --wnaivespacing -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixNaiveSpacing.png`

![Indicatrix Naive Spacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/IndicatrixNaiveSpacing.png)

#### Indicatrix Spacing

`--widget Indicatrix --gridx 60 --gridy 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixSpacing.png`

![Indicatrix Spacing](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/IndicatrixSpacing.png)

#### Indicatrix With Projection

`--widget Indicatrix --projection azimuthal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\IndicatrixWithProjection.png`

![Indicatrix With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/IndicatrixWithProjection.png)



### Local Hours Usage

#### Basic Local Hours

`--widget localhours -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicLocalHours.png`

![Basic Local Hours](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicLocalHours.png)

#### Local Hours Color

`--widget localhours --widgetcolor 128,128,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LocalHoursColor.png`

![Local Hours Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/LocalHoursColor.png)

#### Local Hours Position

`--widget localhours --wlon 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LocalHoursPosition.png`

![Local Hours Position](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/LocalHoursPosition.png)

#### Local Hours With Projection

`--widget temporaryhours --projection orthographic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\LocalHoursWithProjection.png`

![Local Hours With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/LocalHoursWithProjection.png)



### Temporary Hours Usage

#### Basic Temporary Hours

`--widget temporaryhours -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicTemporaryHours.png`

![Basic Temporary Hours](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicTemporaryHours.png)

#### Temporary Hours Color

`--widget temporaryhours --widgetcolor 128,128,255 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TemporaryHoursColor.png`

![Temporary Hours Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/TemporaryHoursColor.png)

#### Temporary Hours Position

`--widget temporaryhours --wlat 60 --wlon 60 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TemporaryHoursPosition.png`

![Temporary Hours Position](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/TemporaryHoursPosition.png)

#### Temporary Hours With Projection

`--widget temporaryhours --projection sinusoidal -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TemporaryHoursWithProjection.png`

![Temporary Hours With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/TemporaryHoursWithProjection.png)



### Tropics Usage

#### Basic Tropics

`--widget Tropics -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\BasicTropics.png`

![Basic Tropics](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/BasicTropics.png)

#### Tropics Color

`--widget Tropics --widgetcolor 128,255,128 -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TropicsColor.png`

![Tropics Color](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/TropicsColor.png)

#### Tropics With Projection

`--widget tropics --projection gnomonic -f ..\..\Tests\Input\earth_equirect.png -o ..\..\Tests\Output\TropicsWithProjection.png`

![Tropics With Projection](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/TropicsWithProjection.png)

