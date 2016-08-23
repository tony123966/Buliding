xof 0302txt 0032
Header {
 1;
 0;
 1;
}
template Header {
 <3D82AB43-62DA-11cf-AB39-0020AF71E433>
 WORD major;
 WORD minor;
 DWORD flags;
}

template Vector {
 <3D82AB5E-62DA-11cf-AB39-0020AF71E433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template Coords2d {
 <F6F23F44-7686-11cf-8F52-0040333594A3>
 FLOAT u;
 FLOAT v;
}

template Matrix4x4 {
 <F6F23F45-7686-11cf-8F52-0040333594A3>
 array FLOAT matrix[16];
}

template ColorRGBA {
 <35FF44E0-6C7C-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <D3E16E81-7835-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template TextureFilename {
 <A42790E1-7810-11cf-8F52-0040333594A3>
 STRING filename;
}

template Material {
 <3D82AB4D-62DA-11cf-AB39-0020AF71E433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshFace {
 <3D82AB5F-62DA-11cf-AB39-0020AF71E433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template MeshTextureCoords {
 <F6F23F40-7686-11cf-8F52-0040333594A3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template MeshMaterialList {
 <F6F23F42-7686-11cf-8F52-0040333594A3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material]
}

template MeshNormals {
 <F6F23F43-7686-11cf-8F52-0040333594A3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template Mesh {
 <3D82AB44-62DA-11cf-AB39-0020AF71E433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

template FrameTransformMatrix {
 <F6F23F41-7686-11cf-8F52-0040333594A3>
 Matrix4x4 frameMatrix;
}

template Frame {
 <3D82AB46-62DA-11cf-AB39-0020AF71E433>
 [...]
}
template FloatKeys {
 <10DD46A9-775B-11cf-8F52-0040333594A3>
 DWORD nValues;
 array FLOAT values[nValues];
}

template TimedFloatKeys {
 <F406B180-7B3B-11cf-8F52-0040333594A3>
 DWORD time;
 FloatKeys tfkeys;
}

template AnimationKey {
 <10DD46A8-775B-11cf-8F52-0040333594A3>
 DWORD keyType;
 DWORD nKeys;
 array TimedFloatKeys keys[nKeys];
}

template AnimationOptions {
 <E2BF56C0-840F-11cf-8F52-0040333594A3>
 DWORD openclosed;
 DWORD positionquality;
}

template Animation {
 <3D82AB4F-62DA-11cf-AB39-0020AF71E433>
 [...]
}

template AnimationSet {
 <3D82AB50-62DA-11cf-AB39-0020AF71E433>
 [Animation]
}

template XSkinMeshHeader {
 <3cf169ce-ff7c-44ab-93c0-f78f62d172e2>
 WORD nMaxSkinWeightsPerVertex;
 WORD nMaxSkinWeightsPerFace;
 WORD nBones;
}

template VertexDuplicationIndices {
 <b8d65549-d7c9-4995-89cf-53a9a8b031e3>
 DWORD nIndices;
 DWORD nOriginalVertices;
 array DWORD indices[nIndices];
}

template SkinWeights {
 <6f0d123b-bad2-4167-a0d0-80224f25fabb>
 STRING transformNodeName;
 DWORD nWeights;
 array DWORD vertexIndices[nWeights];
 array FLOAT weights[nWeights];
 Matrix4x4 matrixOffset;
}
Frame Cylinder01 {
   FrameTransformMatrix {
1.000000,0.000000,0.000000,0.000000,
0.000000,1.000000,0.000000,0.000000,
0.000000,0.000000,1.000000,0.000000,
0.000000,6.686183,0.000000,1.000000;;
 }
Mesh Cylinder011 {
 88;
-95.000000;6.498383;17.854160;,
0.000000;6.498383;17.854160;,
0.000004;-0.000004;19.000000;,
-95.000000;0.000004;19.000000;,
-95.000000;12.212965;14.554844;,
0.000000;12.212965;14.554844;,
-95.000000;16.454483;9.499999;,
0.000000;16.454483;9.499999;,
-95.000000;18.711348;3.299314;,
0.000000;18.711348;3.299314;,
-95.000000;18.711348;-3.299317;,
0.000000;18.711348;-3.299317;,
-95.000000;16.454481;-9.500002;,
0.000000;16.454481;-9.500002;,
-95.000000;12.212962;-14.554846;,
0.000000;12.212962;-14.554846;,
-95.000000;6.498380;-17.854160;,
0.000000;6.498380;-17.854160;,
-95.000000;0.000002;-19.000000;,
0.000004;-0.000006;-19.000000;,
-95.000000;5.130303;14.095389;,
-95.000000;0.000004;15.000000;,
0.000004;-0.000004;15.000000;,
0.000000;5.130303;14.095389;,
-95.000000;9.641814;11.490666;,
0.000000;9.641814;11.490666;,
-95.000000;12.990381;7.499999;,
0.000000;12.990381;7.499999;,
-95.000000;14.772117;2.604721;,
0.000000;14.772117;2.604721;,
-95.000000;14.772116;-2.604724;,
0.000000;14.772116;-2.604724;,
-95.000000;12.990381;-7.500002;,
0.000000;12.990381;-7.500002;,
-95.000000;9.641813;-11.490668;,
0.000000;9.641813;-11.490668;,
-95.000000;5.130300;-14.095390;,
0.000000;5.130300;-14.095390;,
-95.000000;0.000003;-14.999999;,
0.000004;-0.000005;-15.000001;,
-95.000000;0.000002;-19.000000;,
-95.000000;5.130300;-14.095390;,
-95.000000;0.000003;-14.999999;,
-95.000000;6.498380;-17.854160;,
-95.000000;9.641813;-11.490668;,
-95.000000;12.212962;-14.554846;,
-95.000000;12.990381;-7.500002;,
-95.000000;16.454481;-9.500002;,
-95.000000;14.772116;-2.604724;,
-95.000000;18.711348;-3.299317;,
-95.000000;14.772117;2.604721;,
-95.000000;18.711348;3.299314;,
-95.000000;12.990381;7.499999;,
-95.000000;16.454483;9.499999;,
-95.000000;9.641814;11.490666;,
-95.000000;12.212965;14.554844;,
-95.000000;6.498383;17.854160;,
-95.000000;5.130303;14.095389;,
-95.000000;0.000004;19.000000;,
-95.000000;0.000004;15.000000;,
0.000000;12.212965;14.554844;,
0.000000;5.130303;14.095389;,
0.000004;-0.000004;15.000000;,
0.000000;6.498383;17.854160;,
0.000004;-0.000004;19.000000;,
0.000000;16.454483;9.499999;,
0.000000;9.641814;11.490666;,
0.000000;18.711348;3.299314;,
0.000000;12.990381;7.499999;,
0.000000;18.711348;-3.299317;,
0.000000;14.772117;2.604721;,
0.000000;16.454481;-9.500002;,
0.000000;14.772116;-2.604724;,
0.000000;12.212962;-14.554846;,
0.000000;12.990381;-7.500002;,
0.000000;6.498380;-17.854160;,
0.000000;9.641813;-11.490668;,
0.000004;-0.000006;-19.000000;,
0.000000;5.130300;-14.095390;,
0.000004;-0.000005;-15.000001;,
0.000004;-0.000004;15.000000;,
-95.000000;0.000004;15.000000;,
-95.000000;0.000004;19.000000;,
0.000004;-0.000004;19.000000;,
0.000004;-0.000005;-15.000001;,
0.000004;-0.000006;-19.000000;,
-95.000000;0.000002;-19.000000;,
-95.000000;0.000003;-14.999999;;

 76;
3;2,1,0;,
3;0,3,2;,
3;1,5,4;,
3;4,0,1;,
3;5,7,6;,
3;6,4,5;,
3;7,9,8;,
3;8,6,7;,
3;9,11,10;,
3;10,8,9;,
3;11,13,12;,
3;12,10,11;,
3;13,15,14;,
3;14,12,13;,
3;15,17,16;,
3;16,14,15;,
3;17,19,18;,
3;18,16,17;,
3;22,21,20;,
3;20,23,22;,
3;23,20,24;,
3;24,25,23;,
3;25,24,26;,
3;26,27,25;,
3;27,26,28;,
3;28,29,27;,
3;29,28,30;,
3;30,31,29;,
3;31,30,32;,
3;32,33,31;,
3;33,32,34;,
3;34,35,33;,
3;35,34,36;,
3;36,37,35;,
3;37,36,38;,
3;38,39,37;,
3;42,41,40;,
3;44,43,40;,
3;40,41,44;,
3;46,45,43;,
3;43,44,46;,
3;48,47,45;,
3;45,46,48;,
3;50,49,47;,
3;47,48,50;,
3;52,51,49;,
3;49,50,52;,
3;54,53,51;,
3;51,52,54;,
3;57,56,55;,
3;57,55,53;,
3;54,57,53;,
3;59,58,56;,
3;56,57,59;,
3;62,61,60;,
3;62,60,63;,
3;62,63,64;,
3;61,66,65;,
3;65,60,61;,
3;66,68,67;,
3;67,65,66;,
3;68,70,69;,
3;69,67,68;,
3;70,72,71;,
3;71,69,70;,
3;72,74,73;,
3;73,71,72;,
3;74,76,75;,
3;75,73,74;,
3;76,78,77;,
3;77,75,76;,
3;78,79,77;,
3;82,81,80;,
3;80,83,82;,
3;86,85,84;,
3;84,87,86;;
MeshMaterialList {
 1;
 76;
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0;;
Material Material {
 0.752941;0.752941;0.752941;1.000000;;
8.000000;
 0.752941;0.752941;0.752941;;
 0.000000;0.000000;0.000000;;
 }
}

 MeshNormals {
 88;
0.000000;0.286295;0.958142;,
0.000000;0.396567;0.918006;,
0.000000;0.173648;0.984808;,
0.000000;0.173648;0.984808;,
0.000000;0.596733;0.802440;,
0.000000;0.686628;0.727009;,
0.000000;0.835196;0.549952;,
0.000000;0.893871;0.448325;,
0.000000;0.972922;0.231132;,
0.000000;0.993300;0.115566;,
0.000000;0.993300;-0.115566;,
0.000000;0.972922;-0.231132;,
0.000000;0.893870;-0.448325;,
0.000000;0.835196;-0.549952;,
0.000000;0.686627;-0.727010;,
0.000000;0.596733;-0.802440;,
0.000000;0.396567;-0.918006;,
0.000000;0.286295;-0.958142;,
0.000000;0.173648;-0.984808;,
0.000000;0.173648;-0.984808;,
-0.000000;-0.286295;-0.958142;,
-0.000000;-0.173648;-0.984808;,
-0.000000;-0.173648;-0.984808;,
0.000000;-0.396567;-0.918006;,
0.000000;-0.596733;-0.802440;,
0.000000;-0.686628;-0.727009;,
0.000000;-0.835196;-0.549952;,
0.000000;-0.893871;-0.448325;,
0.000000;-0.972922;-0.231132;,
0.000000;-0.993300;-0.115566;,
0.000000;-0.993300;0.115566;,
0.000000;-0.972922;0.231132;,
0.000000;-0.893871;0.448325;,
0.000000;-0.835196;0.549952;,
0.000000;-0.686627;0.727010;,
0.000000;-0.596733;0.802440;,
0.000000;-0.396567;0.918006;,
0.000000;-0.286295;0.958142;,
0.000000;-0.173648;0.984808;,
0.000000;-0.173648;0.984808;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
-1.000000;0.000000;0.000000;,
1.000000;0.000000;-0.000001;,
1.000000;0.000000;-0.000001;,
1.000000;0.000000;-0.000001;,
1.000000;0.000000;0.000000;,
1.000000;0.000001;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;0.000000;,
1.000000;0.000000;-0.000000;,
1.000000;-0.000000;0.000001;,
1.000000;0.000000;0.000001;,
1.000000;-0.000000;0.000001;,
1.000000;0.000001;-0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;,
-0.000000;-1.000000;0.000000;;

 76;
3;2,1,0;,
3;0,3,2;,
3;1,5,4;,
3;4,0,1;,
3;5,7,6;,
3;6,4,5;,
3;7,9,8;,
3;8,6,7;,
3;9,11,10;,
3;10,8,9;,
3;11,13,12;,
3;12,10,11;,
3;13,15,14;,
3;14,12,13;,
3;15,17,16;,
3;16,14,15;,
3;17,19,18;,
3;18,16,17;,
3;22,21,20;,
3;20,23,22;,
3;23,20,24;,
3;24,25,23;,
3;25,24,26;,
3;26,27,25;,
3;27,26,28;,
3;28,29,27;,
3;29,28,30;,
3;30,31,29;,
3;31,30,32;,
3;32,33,31;,
3;33,32,34;,
3;34,35,33;,
3;35,34,36;,
3;36,37,35;,
3;37,36,38;,
3;38,39,37;,
3;42,41,40;,
3;44,43,40;,
3;40,41,44;,
3;46,45,43;,
3;43,44,46;,
3;48,47,45;,
3;45,46,48;,
3;50,49,47;,
3;47,48,50;,
3;52,51,49;,
3;49,50,52;,
3;54,53,51;,
3;51,52,54;,
3;57,56,55;,
3;57,55,53;,
3;54,57,53;,
3;59,58,56;,
3;56,57,59;,
3;62,61,60;,
3;62,60,63;,
3;62,63,64;,
3;61,66,65;,
3;65,60,61;,
3;66,68,67;,
3;67,65,66;,
3;68,70,69;,
3;69,67,68;,
3;70,72,71;,
3;71,69,70;,
3;72,74,73;,
3;73,71,72;,
3;74,76,75;,
3;75,73,74;,
3;76,78,77;,
3;77,75,76;,
3;78,79,77;,
3;82,81,80;,
3;80,83,82;,
3;86,85,84;,
3;84,87,86;;
 }
}
 }