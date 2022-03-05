# Examples

## InvertFromMercator

If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection. Notice that some projections don't include the necessary data to completely recreate an equirect map.

`--projection mercator --invert -f ..\..\Tests\earth_mercator.png -o ..\..\Tests\Output\InvertFromMercator.png`

![InvertFromMercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/InvertFromMercator.png)


## ToLatLong



`--projection LatLong -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToLatLong.png`

![ToLatLong](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToLatLong.png)


## ToEquirect



`--projection Equirect -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToEquirect.png`

![ToEquirect](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEquirect.png)


## ToEquirectangular



`--projection Equirectangular -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToEquirectangular.png`

![ToEquirectangular](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEquirectangular.png)


## ToEqualArea



`--projection EqualArea -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToEqualArea.png`

![ToEqualArea](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToEqualArea.png)


## ToSinusoidal



`--projection Sinusoidal -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal.png`

![ToSinusoidal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToSinusoidal.png)


## ToSinusoidal2



`--projection Sinusoidal2 -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToSinusoidal2.png`

![ToSinusoidal2](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToSinusoidal2.png)


## ToMollweide



`--projection Mollweide -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToMollweide.png`

![ToMollweide](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToMollweide.png)


## ToMercator



`--projection Mercator -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToMercator.png`

![ToMercator](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToMercator.png)


## ToCylindrical



`--projection Cylindrical -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToCylindrical.png`

![ToCylindrical](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToCylindrical.png)


## ToAzimuthal



`--projection Azimuthal -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToAzimuthal.png`

![ToAzimuthal](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToAzimuthal.png)


## ToOrthographic



`--projection Orthographic -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToOrthographic.png`

![ToOrthographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToOrthographic.png)


## ToRectilinear



`--projection Rectilinear -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToRectilinear.png`

![ToRectilinear](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToRectilinear.png)


## ToStereographic



`--projection Stereographic -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToStereographic.png`

![ToStereographic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToStereographic.png)


## ToGnomonic



`--projection Gnomonic -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToGnomonic.png`

![ToGnomonic](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToGnomonic.png)


## ToPerspective



`--projection Perspective -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToPerspective.png`

![ToPerspective](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToPerspective.png)


## ToBonne



`--projection Bonne -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToBonne.png`

![ToBonne](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToBonne.png)


## ToHammer



`--projection Hammer -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\ToHammer.png`

![ToHammer](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/ToHammer.png)


## WithBackgroundColor

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background color

`--bgcolor 255,0,0 --projection perspective -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundColor.png`

![WithBackgroundColor](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WithBackgroundColor.png)


## WithBackgroundImage

Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background image

`--bg ..\..\Tests\background.png --projection hammer -f ..\..\Tests\earth_equirect.png -o ..\..\Tests\Output\WithBackgroundImage.png`

![WithBackgroundImage](https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output/WithBackgroundImage.png)


