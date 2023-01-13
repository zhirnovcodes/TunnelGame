uniform float4x4 _BezierNodes;

float3 getBezierPoint(float t)
{
	float t2 = t * t;
	float t3 = t2 * t;

	float3 p0 = _BezierNodes[0].xyz;
	float3 p1 = _BezierNodes[1].xyz;
	float3 p2 = _BezierNodes[2].xyz;
	float3 p3 = _BezierNodes[3].xyz;

	return
		p0 * (-t3 + 3*t2 - 3*t + 1) +
		p1 * (3*t3 - 6*t2 + 3*t) +
		p2 * (-3*t3 + 3*t2) +
		p3 * (t3);
}

float3 getBezierTangent(float t)
{
	float3 p0 = _BezierNodes[0].xyz;
	float3 p1 = _BezierNodes[1].xyz;
	float3 p2 = _BezierNodes[2].xyz;
	float3 p3 = _BezierNodes[3].xyz;

	float omt = 1.0-t;
	float omt2 = omt * omt;
	float t2 = t * t;
	float3 tangent = 
			p0 * ( -omt2 ) +
			p1 * ( 3 * omt2 - 2 * omt ) +
			p2 * ( -3 * t2 + 2 * t ) +
			p3 * ( t2 );
	return normalize(tangent);
}


float3 getBezierNormal2D( float t ) 
{
	float3 tng = getBezierTangent( t );
	return float3( -tng.y, tng.x, 0.0 );
}

float3 getBezierNormal3D(float t, float3 up)
{
	float3 tng = getBezierTangent( t );
	float3 binormal = normalize(cross( up, tng ));
	return cross( tng, binormal );
}

float getBezierT(float t, sampler2D tMap){
	float x = t % 1;
	int y = (int)t;
	return  tex2Dlod (tMap, float4(x, y, 0, 0));
}

struct ObjectPosNormal
{
	float4 position;
	float3 normal;
	float4 tangent;
};

float4x4 getBezier4Transform(float t, float3 up)
{
	//float3 up = float3(0, 1, 0);

	float3 pos = getBezierPoint(t);

	float3 tan = getBezierTangent(t);
	float3 bit = normalize(cross( up, tan ));
	float3 nor = normalize(cross( tan, bit ));

	return float4x4(float4(bit, 0), float4(nor, 0), float4(tan, 0), float4(pos, 0));//{{bit, 0},{nor, 0},{tan, 0},{0, 0, 0});
	//return float4x4(float4(1,0,0,0), float4(0,1,0, 0), float4(0,0,1, 0), float4(0,0,0, 1));//{{bit, 0},{nor, 0},{tan, 0},{0, 0, 0});
}

float4x4 getBezier3Transform(float t, float3 up)
{
	
    float u = 1 - t;
	
	float3 p0 = _BezierNodes[0].xyz;
	float3 p1 = _BezierNodes[1].xyz;
	float3 p2 = _BezierNodes[2].xyz;

	float3 pos = u * u * p0 + 2 * u * t * p1 + t * t * p2;;

	float3 tan = normalize(2 * u * (p1 - p0) + 2 * t * (p2 - p1));
	float3 nor = normalize(cross(tan, up));
	float3 bit = normalize(cross( nor, tan ));

	return float4x4(float4(bit, 0), float4(nor, 0), float4(tan, 0), float4(pos, 0));
}

float4x4 getBezierTransform(float t, float3 up)
{
	float pointsCount = _BezierNodes[3].w;
	float4x4 bezTr = (pointsCount <= 3) ? getBezier3Transform(t, up) : getBezier4Transform(t, up);
	return bezTr;
}

ObjectPosNormal applyBezierTransform(float4 position, float3 normal, float4x4 transform)
{
    float4 bezPos = transform[3];

    float4 vPos = mul(float4(position.xy, 0, 0), transform);
	float3 vNor = normalize(mul(normal, transform).xyz);

    vPos += bezPos;

	ObjectPosNormal result;
	result.position = vPos;
	result.normal = vNor;
	result.tangent = transform[2];
    return result;
}

ObjectPosNormal changeObjectPositionNormal(float4 position, float3 normal, float2 t, float3 up)
{
	float4x4 bezTr = getBezierTransform(t, up);
	return applyBezierTransform(position, normal, bezTr);
}