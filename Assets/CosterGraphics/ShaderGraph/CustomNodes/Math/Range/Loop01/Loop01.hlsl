void Loop01_float(float In, out float Out)
{
    float v = In;
    if (v < 0.0){
        v += 1.0 - floor(v);
    }
    Out = v - floor(v);
}