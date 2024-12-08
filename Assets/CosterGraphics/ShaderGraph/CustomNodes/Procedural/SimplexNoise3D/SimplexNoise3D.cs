// C# translation of SimplexNoise3D.hlsl

using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public static class SimplexNoise3D
{
    static float3 mod289_3(float3 x){
        return x - math.floor(x / 289.0f) * 289.0f;
    }

    static float4 mod289_4(float4 x){
        return x - math.floor(x / 289.0f) * 289.0f;
    }

    static float4 permute(float4 x){
        return mod289_4((x * 34.0f+ 1.0f) * x);
    }

    static float4 taylorInvSqrt(float4 r){
        return 1.79284291400159f - r * 0.85373472095314f;
    }

    static float4 snoise_grad(float3 v){
        float2 C = new float2(1.0f / 6.0f, 1.0f / 3.0f);
        // First corner
        float3 i  = math.floor(v + math.dot(v, C.yyy));
        float3 x0 = v   - i + math.dot(i, C.xxx);
        // Other corners
        float3 g = math.step(x0.yzx, x0.xyz);
        float3 l = 1.0f - g;
        float3 i1 = math.min(g.xyz, l.zxy);
        float3 i2 = math.max(g.xyz, l.zxy);

        float3 x1 = x0 - i1 + C.xxx;
        float3 x2 = x0 - i2 + C.yyy;
        float3 x3 = x0 - 0.5f;
        // Permutations
        i = mod289_3(i); // Avoid truncation effects in permutation
        float4 p =
            permute(permute(permute(i.z + new float4(0.0f, i1.z, i2.z, 1.0f))
                                  + i.y + new float4(0.0f, i1.y, i2.y, 1.0f))
                                  + i.x + new float4(0.0f, i1.x, i2.x, 1.0f));
        // Gradients: 7x7 points over a square, mapped onto an octahedron.
        // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
        float4 j = p - 49.0f * math.floor(p / 49.0f);  // mod(p,7*7)
        float4 x_ = math.floor(j / 7.0f);
        float4 y_ = math.floor(j - 7.0f * x_);  // mod(j,N)
        float4 x = (x_ * 2.0f + 0.5f) / 7.0f - 1.0f;
        float4 y = (y_ * 2.0f + 0.5f) / 7.0f - 1.0f;
        float4 h = 1.0f - math.abs(x) - math.abs(y);
        float4 b0 = new float4(x.xy, y.xy);
        float4 b1 = new float4(x.zw, y.zw);
        //float4 s0 = float4(lessThan(b0, 0.0)) * 2.0 - 1.0;
        //float4 s1 = float4(lessThan(b1, 0.0)) * 2.0 - 1.0;
        float4 s0 = floor(b0) * 2.0f + 1.0f;
        float4 s1 = floor(b1) * 2.0f + 1.0f;
        float4 sh = -step(h, 0.0f);
        float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
        float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
        float3 g0 = float3(a0.xy, h.x);
        float3 g1 = float3(a0.zw, h.y);
        float3 g2 = float3(a1.xy, h.z);
        float3 g3 = float3(a1.zw, h.w);
        // Normalise gradients
        float4 norm = taylorInvSqrt(math.float4(math.dot(g0, g0), math.dot(g1, g1), math.dot(g2, g2), math.dot(g3, g3)));
        g0 *= norm.x;
        g1 *= norm.y;
        g2 *= norm.z;
        g3 *= norm.w;
        // Compute noise and gradient at P
        float4 m = max(0.6f - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0f);
        float4 m2 = m * m;
        float4 m3 = m2 * m;
        float4 m4 = m2 * m2;
        float3 grad =
            -6.0f * m3.x * x0 * dot(x0, g0) + m4.x * g0 +
            -6.0f * m3.y * x1 * dot(x1, g1) + m4.y * g1 +
            -6.0f * m3.z * x2 * dot(x2, g2) + m4.z * g2 +
            -6.0f * m3.w * x3 * dot(x3, g3) + m4.w * g3;
        float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
        return 42.0f * float4(grad, dot(m4, px));
    }

    static Vector4 EvaluateLayeredNoise(Vector3 p,Vector3 offset, float strength, int octaves, float baseRoughness, float roughness,float persistence){
        float4 noiseVector = 0.0f;
        float frequency = baseRoughness;
        float amplitude  = 1.0f;

        for(int i=0;i<octaves;i++){
            float4 n = snoise_grad(p * frequency + offset);
            noiseVector += (n+1.0f) * 0.5f * amplitude;
            frequency *= roughness;
            amplitude *= persistence;
        }

        return noiseVector * strength;
    }

    public static float SimplexNoise3D_float(Vector3 In){
        float4 noise_vector;
        noise_vector = snoise_grad(In);
        return noise_vector.w;
    }

    public static float SimplexNoise3D_float(float3 In){
        float4 noise_vector;
        noise_vector = snoise_grad(In);
        return noise_vector.w;
    }

    public static void SimplexNoise3D_float(Vector3 In, out float Noise, out Vector3 Gradient){
        float4 noise_vector;
        noise_vector = snoise_grad(In);
        Noise = noise_vector.w;
        Gradient = noise_vector.xyz;
    }

    public static float SimplexNoise3DLayered_float(Vector3 In,Vector3 Offset,float Strength,int Octaves,float BaseRoughness,float Roughness,float Persistence){
        float4 noise_vector;
        noise_vector = EvaluateLayeredNoise(In,Offset,Strength,Octaves,BaseRoughness, Roughness, Persistence);
        return noise_vector.w;
    }

    public static void SimplexNoise3DLayered_float(Vector3 In,Vector3 Offset,float Strength,int Octaves,float BaseRoughness,float Roughness,float Persistence, out float Noise, out Vector3 Gradient){
        float4 noise_vector;
        noise_vector = EvaluateLayeredNoise(In,Offset,Strength,Octaves,BaseRoughness, Roughness, Persistence);
        Noise = noise_vector.w;
        Gradient = noise_vector.xyz;
    }
}


// using UnityEngine;

// public class SimplexNoise3D : MonoBehaviour
// {

//     Vector3 mod289_3(Vector3 x){
//         return x - new Vector3(
//             Mathf.Floor(x.x / 289.0f) * 289.0f,
//             Mathf.Floor(x.y / 289.0f) * 289.0f,
//             Mathf.Floor(x.z / 289.0f) * 289.0f);
//     }

//     float mod289(float x){
//         return x - Mathf.Floor(x / 289.0f) * 289.0f;
//     }

//     Vector4 permute(Vector4 x){
//         return new Vector4( 
//             mod289((x.x * 34.0f + 1.0f) * x.x),
//             mod289((x.y * 34.0f + 1.0f) * x.y),
//             mod289((x.z * 34.0f + 1.0f) * x.z),
//             mod289((x.w * 34.0f + 1.0f) * x.w));
//     }

//     Vector3 taylorInvSqrt(Vector3 r){
//         return new Vector3(
//             1.79284291400159f - r.x * 0.85373472095314f,
//             1.79284291400159f - r.y * 0.85373472095314f,
//             1.79284291400159f - r.z * 0.85373472095314f);
//     }

//     Vector3 Floor(Vector3 v)
//     {
//         return new Vector3(
//             Mathf.Floor(v.x),
//             Mathf.Floor(v.y),
//             Mathf.Floor(v.z));
//     }

//     float Step(float edge, float x)
//     {
//         //  0.0 is returned if x < edge, and 1.0 is returned otherwise.
//         return x < edge? 0.0f: 1.0f;
//     }

//     Vector3 Step(Vector3 edge, Vector3 x)
//     {
//         //  0.0 is returned if x < edge, and 1.0 is returned otherwise.
//         return new Vector3( 
//             x.x < edge.x? 0.0f: 1.0f,
//             x.y < edge.y? 0.0f: 1.0f,
//             x.z < edge.z? 0.0f: 1.0f);
//     }

//     Vector4 snoise_grad(Vector3 v){
//         Vector2 C = new Vector2(1.0f / 6.0f, 1.0f / 3.0f);
//         // First corner
//         Vector3 i  = Floor(new Vector3(
//             v.x + Vector3.Dot(v, new Vector3( C.y,C.y,C.y)),
//             v.y + Vector3.Dot(v, new Vector3( C.y,C.y,C.y)),
//             v.z + Vector3.Dot(v, new Vector3( C.y,C.y,C.y))
//         ));

//         Vector3 x0 = new Vector3(
//             v.x - i.x + Vector3.Dot(i, new Vector3(C.x,C.x,C.x)),
//             v.y - i.y + Vector3.Dot(i, new Vector3(C.x,C.x,C.x)),
//             v.z - i.z + Vector3.Dot(i, new Vector3(C.x,C.x,C.x))
//             );

//         // Other corners
//         Vector3 g = Step(new Vector3(x0.y, x0.z,x0.x), x0);
//         Vector3 l = 1.0 - g;
//         Vector3 i1 = min(g.xyz, l.zxy);
//         Vector3 i2 = max(g.xyz, l.zxy);

//         Vector3 x1 = x0 - i1 + C.xxx;
//         Vector3 x2 = x0 - i2 + C.yyy;
//         Vector3 x3 = x0 - 0.5;

//         // Permutations
//         i = mod289_3(i); // Avoid truncation effects in permutation
//         Vector4 p =
//             permute(permute(permute(i.z + Vector4(0.0, i1.z, i2.z, 1.0))
//                                 + i.y + Vector4(0.0, i1.y, i2.y, 1.0))
//                                 + i.x + Vector4(0.0, i1.x, i2.x, 1.0));
//         // Gradients: 7x7 points over a square, mapped onto an octahedron.
//         // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
//         Vector4 j = p - 49.0 * floor(p / 49.0);  // mod(p,7*7)
//         Vector4 x_ = floor(j / 7.0);
//         Vector4 y_ = floor(j - 7.0 * x_);  // mod(j,N)
//         Vector4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
//         Vector4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;
//         Vector4 h = 1.0 - abs(x) - abs(y);
//         Vector4 b0 = Vector4(x.xy, y.xy);
//         Vector4 b1 = Vector4(x.zw, y.zw);
//         //Vector4 s0 = Vector4(lessThan(b0, 0.0)) * 2.0 - 1.0;
//         //Vector4 s1 = Vector4(lessThan(b1, 0.0)) * 2.0 - 1.0;
//         Vector4 s0 = floor(b0) * 2.0 + 1.0;
//         Vector4 s1 = floor(b1) * 2.0 + 1.0;
//         Vector4 sh = -step(h, 0.0);
//         Vector4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
//         Vector4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
//         Vector3 g0 = Vector3(a0.xy, h.x);
//         Vector3 g1 = Vector3(a0.zw, h.y);
//         Vector3 g2 = Vector3(a1.xy, h.z);
//         Vector3 g3 = Vector3(a1.zw, h.w);
//         // Normalise gradients
//         Vector4 norm = taylorInvSqrt(Vector4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3)));
//         g0 *= norm.x;
//         g1 *= norm.y;
//         g2 *= norm.z;
//         g3 *= norm.w;
//         // Compute noise and gradient at P
//         Vector4 m = max(0.6 - Vector4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
//         Vector4 m2 = m * m;
//         Vector4 m3 = m2 * m;
//         Vector4 m4 = m2 * m2;
//         Vector3 grad =
//             -6.0 * m3.x * x0 * dot(x0, g0) + m4.x * g0 +
//             -6.0 * m3.y * x1 * dot(x1, g1) + m4.y * g1 +
//             -6.0 * m3.z * x2 * dot(x2, g2) + m4.z * g2 +
//             -6.0 * m3.w * x3 * dot(x3, g3) + m4.w * g3;
//         Vector4 px = Vector4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
//         return 42.0 * Vector4(grad, dot(m4, px));
//     }

//     Vector4 EvaluateLayeredNoise(Vector3 p,Vector3 offset, float strength, int octaves, float baseRoughness, float roughness,float persistence){
//         Vector4 noiseVector = 0.0;
//         float frequency = baseRoughness;
//         float amplitude  = 1.0;

//         for(int i=0;i<octaves;i++){
//             Vector4 n = snoise_grad(p * frequency + offset);
//             noiseVector += (n+1.0) * 0.5 * amplitude;
//             frequency *= roughness;
//             amplitude *= persistence;
//         }

//         return noiseVector * strength;
//     }

//     void SimplexNoise3D_float(Vector3 In, out float Noise, out Vector3 Gradient){
//         Vector4 noise_vector;
//             noise_vector = snoise_grad(In);
//             Noise = noise_vector.w;
//             Gradient = noise_vector.xyz;
//     }

//     void SimplexNoise3DLayered_float(Vector3 In,Vector3 Offset,float Strength,int Octaves,float BaseRoughness,float Roughness,float Persistence, out float Noise, out Vector3 Gradient){
//         Vector4 noise_vector;
//         noise_vector = EvaluateLayeredNoise(In,Offset,Strength,Octaves,BaseRoughness, Roughness, Persistence);
//         Noise = noise_vector.w;
//         Gradient = noise_vector.xyz;
//     }
// }
