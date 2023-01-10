float _Permutations[512];

int inc(int num) {
	num++;
		
	return num;
}
	
float grad(int hash, double x, double y, double z) {
	int h = hash & 15;									// Take the hashed value and take the first 4 bits of it (15 == 0b1111)
	float u = h < 8 /* 0b1000 */ ? x : y;				// If the most significant bit (MSB) of the hash is 0 then set u = x.  Otherwise y.
		
	float v;											// In Ken Perlin's original implementation this was another conditional operator (?:).  I
														// expanded it for readability.
		
	if(h < 4 /* 0b0100 */)								// If the first and second significant bits are 0 set v = y
		v = y;
	else if(h == 12 /* 0b1100 */ || h == 14 /* 0b1110*/)// If the first and second significant bits are 1 set v = x
		v = x;
	else 												// If the first and second significant bits are not equal (0/1, 1/0) set v = z
		v = z;
		
	return ((h&1) == 0 ? u : -u)+((h&2) == 0 ? v : -v); // Use the last 2 bits to decide if u and v are positive or negative.  Then return their addition.
}
	
float fade(double t) {
														// Fade function as defined by Ken Perlin.  This eases coordinate values
														// so that they will "ease" towards integral values.  This ends up smoothing
														// the final output.
	return t * t * t * (t * (t * 6 - 15) + 10);			// 6t^5 - 15t^4 + 10t^3
}

float perm(int v1, int v2, int v3){
	return _Permutations[_Permutations[_Permutations[v1] + v2] + v3];
}

float getPerlinNoise(float3 position){
	//if(repeat > 0) {									// If we have any repeat on, change the coordinates to their "local" repetitions
	//		x = x;
	//		y = y%repeat;
	//		z = z%repeat;
	//	}
	float x = position.x;
	float y = position.y;
	float z = position.z;

	int xint = int(x);
	int yint = int(y);
	int zint = int(z);

	int xi = xint & 255;								// Calculate the "unit cube" that the point asked will be located in
	int yi = yint & 255;								// The left bound is ( |_x_|,|_y_|,|_z_| ) and the right bound is that
	int zi = zint & 255;								// plus 1.  Next we calculate the location (from 0.0 to 1.0) in that cube.
	float xf = x - xint;								// We also fade the location to smooth the result.
	float yf = y - yint;
	float zf = z - zint;
	float u = fade(xf);
	float v = fade(yf);
	float w = fade(zf);
															
	int aaa, aba, aab, abb, baa, bba, bab, bbb;
	aaa = perm(xi, yi, zi);//p[p[p[    xi ]+    yi ]+    zi ];
	aba = perm(xi, inc(yi), zi);//p[p[p[    xi ]+inc(yi)]+    zi ];
	aab = perm(xi, yi, inc(zi));//p[p[p[    xi ]+    yi ]+inc(zi)];

	abb = perm(xi, inc(yi), inc(zi));//p[p[p[    xi ]+inc(yi)]+inc(zi)];
	baa = perm(inc(xi), yi, zi);//p[p[p[inc(xi)]+    yi ]+    zi ];
	bba = perm(inc(xi), inc(yi), zi);//p[p[p[inc(xi)]+inc(yi)]+    zi ];
	bab = perm(inc(xi), yi, inc(zi));//p[p[p[inc(xi)]+    yi ]+inc(zi)];
	bbb = perm(inc(xi), inc(yi), inc(zi));//p[p[p[inc(xi)]+inc(yi)]+inc(zi)];
	
	double x1, x2, y1, y2;
	x1 = lerp(	grad (aaa, xf  , yf  , zf),				// The gradient function calculates the dot product between a pseudorandom
				grad (baa, xf-1, yf  , zf),				// gradient vector and the vector from the input coordinate to the 8
				u);										// surrounding points in its unit cube.
	x2 = lerp(	grad (aba, xf  , yf-1, zf),				// This is all then lerped together as a sort of weighted average based on the faded (u,v,w)
				grad (bba, xf-1, yf-1, zf),				// values we made earlier.
			        u);
	y1 = lerp(x1, x2, v);

	x1 = lerp(	grad (aab, xf  , yf  , zf-1),
				grad (bab, xf-1, yf  , zf-1),
				u);
	x2 = lerp(	grad (abb, xf  , yf-1, zf-1),
		        grad (bbb, xf-1, yf-1, zf-1),
		        u);
	y2 = lerp (x1, x2, v);
		
	return (lerp (y1, y2, w)+1)/2;			
}

float getPerlinNoiseOctaves(int octaves, float persistence, float3 position){
	float total = 0;
    float frequency = 1;
    float amplitude = 1;
    float maxValue = 0;

    for(int i=0;i<octaves;i++) {
        total += getPerlinNoise(position * frequency) * amplitude;
        
        maxValue += amplitude;
        
        amplitude *= persistence;
        frequency *= 2;
    }
    
    return total/maxValue;
}
