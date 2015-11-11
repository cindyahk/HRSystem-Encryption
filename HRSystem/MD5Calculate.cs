using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DWORD = System.UInt32;

/*We begin by supposing that we have a b-bit message as input, and that
   we wish to find its message digest. Here b is an arbitrary
   nonnegative integer; b may be zero, it need not be a multiple of
   eight, and it may be arbitrarily large. We imagine the bits of the
   message written down as follows:          m_0 m_1 ... m_{b-1}.

   In this document a "dword" is a 32-bit quantity and a "byte" is an
   eight-bit quantity. A sequence of bits can be interpreted in a
   natural manner as a sequence of bytes, where each consecutive group
   of eight bits is interpreted as a byte with the high-order (most
   significant) bit of each byte listed first. Similarly, a sequence of
   bytes can be interpreted as a sequence of 32-bit dwords, where each
   consecutive group of four bytes is interpreted as a dword with the
   low-order (least significant) byte given first.

   The following five steps are performed to compute the message digest
   of the message.
   
   Step 1. Append Padding Bits
   The message is "padded" (extended) so that its length (in bits) is
   congruent to 448, modulo 512. That is, the message is extended so
   that it is just 64 bits shy of being a multiple of 512 bits long.
   Padding is always performed, even if the length of the message is
   already congruent to 448, modulo 512.
   Padding is performed as follows: a single "1" bit is appended to the
   message, and then "0" bits are appended so that the length in bits of
   the padded message becomes congruent to 448, modulo 512. In all, at
   least one bit and at most 512 bits are appended.

   Step 2. Append Length
   A 64-bit representation of b (the length of the message before the
   padding bits were added) is appended to the result of the previous
   step. In the unlikely event that b is greater than 2^64, then only
   the low-order 64 bits of b are used. (These bits are appended as two
   32-bit words and appended low-order word first in accordance with the
   previous conventions.)
   At this point the resulting message (after padding with bits and with
   b) has a length that is an exact multiple of 512 bits. Equivalently,
   this message has a length that is an exact multiple of 16 (32-bit)
   dwords. Let M[0 ... N-1] denote the dwords of the resulting message,
   where N is a multiple of 16.
   
   Step 3. Initialize MD Buffer
   A four-dword buffer (A,B,C,D) is used to compute the message digest.
   Here each of A, B, C, D is a 32-bit register. These registers are
   initialized to the following values in hexadecimal):
          dword A: 0x67452301          dword B: 0xEFCDAB89
          dword C: 0x98BADCFE          dword D: 0x10325476

   Step 4. Process Message in 16-Word Blocks
   We first define four auxiliary functions that each take as input
   three 32-bit dwords and produce as output one 32-bit dword.
          F(X,Y,Z) = XY v (not(X) & Z)      G(X,Y,Z) = XZ v (Y& not(Z))
          H(X,Y,Z) = X xor Y xor Z          I(X,Y,Z) = Y xor (X v not(Z))
   In each bit position F acts as a conditional: if X then Y else Z.
   The function F could have been defined using + instead of v since XY
   and not(X)Z will never have 1's in the same bit position.) It is
   interesting to note that if the bits of X, Y, and Z are independent
   and unbiased, the each bit of F(X,Y,Z) will be independent and   unbiased.
   The functions G, H, and I are similar to the function F, in that they
   act in "bitwise parallel" to produce their output from the bits of X,
   Y, and Z, in such a manner that if the corresponding bits of X, Y,
   and Z are independent and unbiased, then each bit of G(X,Y,Z),
   H(X,Y,Z), and I(X,Y,Z) will be independent and unbiased. Note that
   the function H is the bit-wise "xor" or "parity" function of its   inputs.
   This step uses a 64-element table T[1 ... 64] constructed from the
   sine function. Let T[i] denote the i-th element of the table, which
   is equal to the integer part of 4294967296 times abs(sin(i)), where i
   is in radians. The elements of the table are given in the report.
   <<< s denotes a circular shift by s number of bits.
   Do the following:   
   
	 //Process each 16-dword block.
     For i = 0 to (N/16)-1 do     // Copy block i into X. N is the number of sub-blocks, thus i is the number of 16-dword sub-blocks
		For j = 0 to 15 do       //for each sublock
			Set X[j] to M[i*16+j].     
        end //of loop on j

		 // Save A as AA, B as BB, C as CC, and D as DD.
		 AA = A     BB = B
		 CC = C     DD = D     

		 // Round 1.
		 // Let [abcd k s i] denote the operation
		 // a = b + ((a + F(b,c,d) + X[k] + T[i]) <<< s), where X[k] is one sub-block, k is index of sub-block
		 // Do the following 16 operations.
		 [ABCD  0  7  1]  [DABC  1 12  2]  [CDAB  2 17  3]  [BCDA  3 22  4]
		 [ABCD  4  7  5]  [DABC  5 12  6]  [CDAB  6 17  7]  [BCDA  7 22  8]
		 [ABCD  8  7  9]  [DABC  9 12 10]  [CDAB 10 17 11]  [BCDA 11 22 12]
		 [ABCD 12  7 13]  [DABC 13 12 14]  [CDAB 14 17 15]  [BCDA 15 22 16]

		 // Round 2.      
		 // Let [abcd k s i] denote the operation 
		 // a = b + ((a + G(b,c,d) + X[k] + T[i]) <<< s).
		 // Do the following 16 operations.
		 [ABCD  1  5 17]  [DABC  6  9 18]  [CDAB 11 14 19]  [BCDA  0 20 20]
		 [ABCD  5  5 21]  [DABC 10  9 22]  [CDAB 15 14 23]  [BCDA  4 20 24]
		 [ABCD  9  5 25]  [DABC 14  9 26]  [CDAB  3 14 27]  [BCDA  8 20 28]
		 [ABCD 13  5 29]  [DABC  2  9 30]  [CDAB  7 14 31]  [BCDA 12 20 32]

		 // Round 3.      
		 // Let [abcd k s t] denote the operation
		 // a = b + ((a + H(b,c,d) + X[k] + T[i]) <<< s).
		 // Do the following 16 operations.
		 [ABCD  5  4 33]  [DABC  8 11 34]  [CDAB 11 16 35]  [BCDA 14 23 36]
		 [ABCD  1  4 37]  [DABC  4 11 38]  [CDAB  7 16 39]  [BCDA 10 23 40]
		 [ABCD 13  4 41]  [DABC  0 11 42]  [CDAB  3 16 43]  [BCDA  6 23 44]
		 [ABCD  9  4 45]  [DABC 12 11 46]  [CDAB 15 16 47]  [BCDA  2 23 48]

		 // Round 4. 
		 // Let [abcd k s t] denote the operation
		 // a = b + ((a + I(b,c,d) + X[k] + T[i]) <<< s).
		 // Do the following 16 operations.
		 [ABCD  0  6 49]  [DABC  7 10 50]  [CDAB 14 15 51]  [BCDA  5 21 52]
		 [ABCD 12  6 53]  [DABC  3 10 54]  [CDAB 10 15 55]  [BCDA  1 21 56]
		 [ABCD  8  6 57]  [DABC 15 10 58]  [CDAB  6 15 59]  [BCDA 13 21 60]
		 [ABCD  4  6 61]  [DABC 11 10 62]  [CDAB  2 15 63]  [BCDA  9 21 64]

		 // Then perform the following additions. (That is increment each
		 //   of the four registers by the value it had before this block
		 //   was started.) 
		A = A + AA     B = B + BB     C = C + CC  D = D + DD   

	end // of loop on i

   Step 5. Output
   The message digest produced as output is A, B, C, D. That is, we
   begin with the low-order byte of A, and end with the high-order byte of D.
   This completes the description of MD5.


*/
namespace MD5
{
    class MD5Calculate
    {
        private byte[]  m_lpszBuffer = new byte[64];  //input buffer, input message is 512 bits (1 byte * 64)
        private DWORD[] m_nCount = new DWORD[2];   //number of bits, modulo 2^64 (lsb first)
        private uint[]  m_lMD5 = new uint[4];   //MD5 checksum, output is 4 blocks of 32 bits

        //Magic initialization constants
        public const DWORD MD5_INIT_STATE_0 = 0x67452301;
        public const DWORD MD5_INIT_STATE_1 = 0xefcdab89;
        public const DWORD MD5_INIT_STATE_2 = 0x98badcfe;
        public const DWORD MD5_INIT_STATE_3 = 0x10325476;

        //constants for Transform routine.Specifies the per-round shift amounts, there is no need to write
        //all the per-round shift rounds. The first 4 numbers repeat again for the next 4 times in each round
        public const DWORD MD5_S11 = 7;
        public const DWORD MD5_S12 = 12;
        public const DWORD MD5_S13 = 17;
        public const DWORD MD5_S14 = 22;
        //for example, MD5_S15,MD5_S16,MD5_S17,S18 are same as S11,S12,S13,S14 (7,12,17,22)
        public const DWORD MD5_S21 = 5;
        public const DWORD MD5_S22 = 9;
        public const DWORD MD5_S23 = 14;
        public const DWORD MD5_S24 = 20;
        public const DWORD MD5_S31 = 4;
        public const DWORD MD5_S32 = 11;
        public const DWORD MD5_S33 = 16;
        public const DWORD MD5_S34 = 23;
        public const DWORD MD5_S41 = 6;
        public const DWORD MD5_S42 = 10;
        public const DWORD MD5_S43 = 15;
        public const DWORD MD5_S44 = 21;

        //Transformation constants - Round 1
        public const DWORD MD5_T01 = 0xd76aa478; //Transformation Constant 1
        public const DWORD MD5_T02 = 0xe8c7b756; //Transformation Constant 2
        public const DWORD MD5_T03 = 0x242070db; //Transformation Constant 3
        public const DWORD MD5_T04 = 0xc1bdceee;//Transformation Constant 4
        public const DWORD MD5_T05 = 0xf57c0faf; //Transformation Constant 5
        public const DWORD MD5_T06 = 0x4787c62a; //Transformation Constant 6
        public const DWORD MD5_T07 = 0xa8304613; //Transformation Constant 7
        public const DWORD MD5_T08 = 0xfd469501; //Transformation Constant 8
        public const DWORD MD5_T09 = 0x698098d8; //Transformation Constant 9
        public const DWORD MD5_T10 = 0x8b44f7af; //Transformation Constant 10
        public const DWORD MD5_T11 = 0xffff5bb1; //Transformation Constant 11
        public const DWORD MD5_T12 = 0x895cd7be; //Transformation Constant 12
        public const DWORD MD5_T13 = 0x6b901122; //Transformation Constant 13
        public const DWORD MD5_T14 = 0xfd987193; //Transformation Constant 14
        public const DWORD MD5_T15 = 0xa679438e; //Transformation Constant 15
        public const DWORD MD5_T16 = 0x49b40821; //Transformation Constant 16

        //Transformation Constants - Round 2
        public const DWORD MD5_T17 = 0xf61e2562; //Transformation Constant 17
        public const DWORD MD5_T18 = 0xc040b340;//Transformation Constant 18
        public const DWORD MD5_T19 = 0x265e5a51; //Transformation Constant 19
        public const DWORD MD5_T20 = 0xe9b6c7aa; //Transformation Constant 20
        public const DWORD MD5_T21 = 0xd62f105d; //Transformation Constant 21
        public const DWORD MD5_T22 = 0x02441453; //Transformation Constant 22
        public const DWORD MD5_T23 = 0xd8a1e681; //Transformation Constant 23
        public const DWORD MD5_T24 = 0xe7d3fbc8; //Transformation Constant 24
        public const DWORD MD5_T25 = 0x21e1cde6; //Transformation Constant 25
        public const DWORD MD5_T26 = 0xc33707d6; //Transformation Constant 26
        public const DWORD MD5_T27 = 0xf4d50d87; //Transformation Constant 27
        public const DWORD MD5_T28 = 0x455a14ed; //Transformation Constant 28
        public const DWORD MD5_T29 = 0xa9e3e905; //Transformation Constant 29
        public const DWORD MD5_T30 = 0xfcefa3f8; //Transformation Constant 30
        public const DWORD MD5_T31 = 0x676f02d9; //Transformation Constant 31
        public const DWORD MD5_T32 = 0x8d2a4c8a; //Transformation Constant 32

        //Transformation Constants - Round 3
        public const DWORD MD5_T33 = 0xfffa3942; //Transformation Constant 33
        public const DWORD MD5_T34 = 0x8771f681; //Transformation Constant 34
        public const DWORD MD5_T35 = 0x6d9d6122; //Transformation Constant 35
        public const DWORD MD5_T36 = 0xfde5380c; //Transformation Constant 36
        public const DWORD MD5_T37 = 0xa4beea44; //Transformation Constant 37
        public const DWORD MD5_T38 = 0x4bdecfa9; //Transformation Constant 38
        public const DWORD MD5_T39 = 0xf6bb4b60; //Transformation Constant 39
        public const DWORD MD5_T40 = 0xbebfbc70; //Transformation Constant 40
        public const DWORD MD5_T41 = 0x289b7ec6; //Transformation Constant 41
        public const DWORD MD5_T42 = 0xeaa127fa; //Transformation Constant 42
        public const DWORD MD5_T43 = 0xd4ef3085; //Transformation Constant 43
        public const DWORD MD5_T44 = 0x04881d05; //Transformation Constant 44
        public const DWORD MD5_T45 = 0xd9d4d039; //Transformation Constant 45
        public const DWORD MD5_T46 = 0xe6db99e5; //Transformation Constant 46
        public const DWORD MD5_T47 = 0x1fa27cf8; //Transformation Constant 47
        public const DWORD MD5_T48 = 0xc4ac5665; //Transformation Constant 48

        //Transformation Constants - Round 4
        public const DWORD MD5_T49 = 0xf4292244; //Transformation Constant 49
        public const DWORD MD5_T50 = 0x432aff97; //Transformation Constant 50
        public const DWORD MD5_T51 = 0xab9423a7; //Transformation Constant 51
        public const DWORD MD5_T52 = 0xfc93a039; //Transformation Constant 52
        public const DWORD MD5_T53 = 0x655b59c3; //Transformation Constant 53
        public const DWORD MD5_T54 = 0x8f0ccc92; //Transformation Constant 54
        public const DWORD MD5_T55 = 0xffeff47d; //Transformation Constant 55
        public const DWORD MD5_T56 = 0x85845dd1; //Transformation Constant 56
        public const DWORD MD5_T57 = 0x6fa87e4f; //Transformation Constant 57
        public const DWORD MD5_T58 = 0xfe2ce6e0; //Transformation Constant 58
        public const DWORD MD5_T59 = 0xa3014314; //Transformation Constant 59
        public const DWORD MD5_T60 = 0x4e0811a1; //Transformation Constant 60
        public const DWORD MD5_T61 = 0xf7537e82; //Transformation Constant 61
        public const DWORD MD5_T62 = 0xbd3af235; //Transformation Constant 62
        public const DWORD MD5_T63 = 0x2ad7d2bb; //Transformation Constant 63
        public const DWORD MD5_T64 = 0xeb86d391; //Transformation Constant 64


        //Null data (except for first BYTE) used to finalise the checksum calculation
        //Pad the data so that length of message is a multiple of 512 bits, minus 64 bits. 
        //The last remaining bits are
        //filled with a 64-bit little endian value, representing length of original message in bits
        byte[] PADDING =
        {
          0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        public MD5Calculate()
        {
            m_nCount[0] = m_nCount[1] = 0;

            // Load magic state initialization constants
            m_lMD5[0] = MD5_INIT_STATE_0;
            m_lMD5[1] = MD5_INIT_STATE_1;
            m_lMD5[2] = MD5_INIT_STATE_2;
            m_lMD5[3] = MD5_INIT_STATE_3;
        }

        public String GetMD5(byte[] pBuf, uint nLength)
        {
         //calculate and return the checksum         
         Update( pBuf, nLength );
         return Final();
        }

        //Append padding and 64-bit little-endian integer value original length of message
            //Output the message digest in hex form
        public String Final()
            {
             //Save number of bits
             //Original Length of message is stored into m_nCount (which is 2 cells of 32 bits each)
             //Need to change to a 64-bit little-endian integer value (lowest byte first) to be appended byte-by-byte to the last remaining incomplete block
             byte[] Bits=new byte[8]; //a 64-bit little endian integer value
             DWordToByte( Bits, m_nCount, 8 );
	
             //Pad out to 56 mod 64.
             uint nIndex = (uint)((m_nCount[0] >> 3) & 0x3f);
             uint nPadLen = (nIndex < 56) ? (56 - nIndex) : (120 - nIndex);
             Update( PADDING, nPadLen ); //append the padding of 0x8000... bits of length nPadLen to the last remaining incomplete block
	
             //Append original length of message in bits
             Update( Bits, 8 );

             //Store final state in 'lpszMD5'
             //m_lMD5 is 4 DWORD (four 32-bit cells), that is 16 8-bit cells
             const int nMD5Size = 16;
             byte[] lpszMD5=new byte[ nMD5Size ];
             DWordToByte( lpszMD5, m_lMD5, nMD5Size );

             //Convert the hexadecimal checksum to a CString
             String strMD5="";
             for ( int i=0; i < nMD5Size; i++)
             {
              StringBuilder Str = new StringBuilder();
              if (lpszMD5[i] == 0) {
               Str.Append("00");
              }
              else if (lpszMD5[i] <= 15)  {
                  Str.Append("0");
                  Str.Append(lpszMD5[i].ToString("X"));
              }
              else {
               Str.Append(lpszMD5[i].ToString("X"));
              }

             
              strMD5 += Str.ToString();
             }
            
             return strMD5;
 


            }

        public void Update(byte[] Input, uint nInputLen)
        {
            byte[] tempInput = new byte[64];

            //Compute number of bytes mod 64(length in bytes of last remaining incomplete block)
            int nIndex = (int)((m_nCount[0] >> 3) & 0x3F);

            //Update number of bits
            if ((m_nCount[0] += nInputLen << 3) < (nInputLen << 3))
            {
                m_nCount[1]++; //add the carry bit
            }
            m_nCount[1] += (nInputLen >> 29); //m_nCount[1] is incremented by the value of the three high-order  bits of nInputLen (32 - 29 == 3)


            //Transform as many times as possible.
            int i = 0;
            int nPartLen = 64 - nIndex; //nIndex is current length of last remaining incomplete block, 64 bytes is total final length of message block,
                                        //hence nPartLen is length left to be filled

            //if nInputLen is greater than or equal to nPartLen, break the message into 64-byte block and process each one in turn
            
            if (nInputLen >= nPartLen)
            {

                Array.Copy(Input,0,m_lpszBuffer,nIndex,nPartLen);
                Transform(m_lpszBuffer);

                //if the original message contains more than 1 chunk of 64 bytes, break the message in 64-bytes blocks and transform each 64-byte block in turn
                for (i = nPartLen; i + 63 < nInputLen; i += 64)
                {
                    Array.Copy(Input,i,tempInput,0,64);
                    Transform(tempInput);
                }
                nIndex = 0;
            }
            else
            {
                i = 0;
            }


            // Buffer remaining input
            Array.Copy(Input, i, m_lpszBuffer, nIndex, nInputLen - i);

        }

        public void Transform(byte[] Block)
    {
        //initialise local data with current checksum
        DWORD a = m_lMD5[0];
        DWORD b = m_lMD5[1];
        DWORD c = m_lMD5[2];
        DWORD d = m_lMD5[3];

        //copy BYTES from input 'Block' to an array of 'X'
        //Break the 64-byte Block to sixteen 32-bits sub-blocks
        DWORD[] X = new DWORD[16]; //16 sub-blocks, each of 32 bits
        ByteToDWord(X,Block, 64 );

        //Perform Round 1 of the transformation
        // a = b + ((a + F(b,c,d) + X[k] + T[i]) <<< s), where X[k] is one sub-block, k is index of sub-block
        FF(ref a, ref b, ref c, ref d, ref X[0], MD5_S11, MD5_T01);
        FF (ref d, ref a, ref b, ref c,ref X[ 1],MD5_S12, MD5_T02);
        FF (ref c, ref d, ref a, ref b,ref X[ 2], MD5_S13, MD5_T03);
        FF(ref b, ref c, ref d, ref a, ref X[3], MD5_S14, MD5_T04);
        FF(ref a, ref b, ref c, ref d, ref X[4], MD5_S11,  MD5_T05);
        FF(ref d, ref a, ref  b, ref c, ref X[5], MD5_S12, MD5_T06);
        FF(ref c, ref d, ref a, ref b, ref X[6], MD5_S13, MD5_T07);
        FF(ref b, ref c, ref d, ref a, ref X[7], MD5_S14, MD5_T08);
        FF(ref a, ref b, ref c, ref d, ref X[8], MD5_S11, MD5_T09);
        FF(ref d, ref a, ref b, ref c, ref X[9], MD5_S12, MD5_T10);
        FF(ref c, ref d, ref a, ref b, ref X[10], MD5_S13, MD5_T11);
        FF(ref b, ref c, ref d, ref a, ref X[11], MD5_S14, MD5_T12);
        FF(ref a, ref b, ref c, ref d, ref X[12], MD5_S11, MD5_T13);
        FF(ref d, ref a, ref b, ref c, ref X[13], MD5_S12, MD5_T14);
        FF(ref c, ref d, ref a, ref b, ref X[14], MD5_S13, MD5_T15);
        FF(ref b, ref c, ref d, ref a, ref X[15], MD5_S14, MD5_T16);

        //Perform Round 2 of the transformation
        GG(ref a, ref b, ref c, ref d, ref X[1], MD5_S21, MD5_T17);
        GG(ref d, ref a, ref b, ref c, ref X[6], MD5_S22, MD5_T18);
        GG(ref c, ref d,ref a, ref b, ref X[11], MD5_S23, MD5_T19);
        GG(ref b, ref c, ref d,ref a, ref X[0], MD5_S24, MD5_T20);
        GG(ref a, ref b, ref c, ref d, ref X[5], MD5_S21, MD5_T21);
        GG(ref d, ref a, ref b, ref c, ref X[10], MD5_S22, MD5_T22);
        GG (ref c, ref d, ref a, ref b, ref X[15], MD5_S23, MD5_T23);
        GG (ref b, ref c, ref d, ref a, ref X[ 4], MD5_S24, MD5_T24);
        GG (ref a, ref b, ref c, ref d, ref X[ 9], MD5_S21, MD5_T25);
        GG (ref d, ref a, ref b, ref c, ref X[14], MD5_S22, MD5_T26);
        GG (ref c, ref d, ref a, ref b, ref X[ 3], MD5_S23, MD5_T27);
        GG (ref b, ref c, ref d, ref a, ref X[ 8], MD5_S24, MD5_T28);
        GG (ref a, ref b, ref c, ref d, ref X[13], MD5_S21, MD5_T29);
        GG (ref d, ref a, ref b, ref c, ref X[ 2], MD5_S22, MD5_T30);
        GG (ref c, ref d, ref a, ref b, ref X[ 7], MD5_S23, MD5_T31);
        GG (ref b, ref c, ref d, ref a, ref X[12], MD5_S24, MD5_T32);

        //Perform Round 3 of the transformation
        HH (ref a, ref b, ref c, ref d, ref X[ 5], MD5_S31, MD5_T33);
        HH (ref d, ref a, ref b, ref c, ref X[ 8], MD5_S32, MD5_T34);
        HH (ref c, ref d, ref a, ref b, ref X[11], MD5_S33, MD5_T35);
        HH (ref b, ref c, ref d, ref a, ref X[14], MD5_S34, MD5_T36);
        HH (ref a, ref b, ref c, ref d, ref X[ 1], MD5_S31, MD5_T37);
        HH (ref d, ref a, ref b, ref c, ref X[ 4], MD5_S32, MD5_T38);
        HH (ref c, ref d, ref a, ref b, ref X[ 7], MD5_S33, MD5_T39);
        HH (ref b, ref c, ref d, ref a, ref X[10], MD5_S34, MD5_T40);
        HH (ref a, ref b, ref c, ref d, ref X[13], MD5_S31, MD5_T41);
        HH (ref d, ref a, ref b, ref c, ref X[ 0], MD5_S32, MD5_T42);
        HH (ref c, ref d, ref a, ref b, ref X[ 3], MD5_S33, MD5_T43);
        HH (ref b, ref c, ref d, ref a, ref X[ 6], MD5_S34, MD5_T44);
        HH (ref a, ref b, ref c, ref d, ref X[ 9], MD5_S31, MD5_T45);
        HH (ref d, ref a, ref b, ref c, ref X[12], MD5_S32, MD5_T46);
        HH (ref c, ref d, ref a, ref b, ref X[15], MD5_S33, MD5_T47);
        HH (ref b, ref c, ref d, ref a, ref X[ 2], MD5_S34, MD5_T48);

        //Perform Round 4 of the transformation
        II (ref a, ref b, ref c, ref d, ref X[ 0], MD5_S41, MD5_T49);
        II (ref d, ref a, ref b, ref c, ref X[ 7], MD5_S42, MD5_T50);
        II (ref c, ref d, ref a, ref b, ref X[14], MD5_S43, MD5_T51);
        II (ref b, ref c, ref d, ref a, ref X[ 5], MD5_S44, MD5_T52);
        II (ref a, ref b, ref c, ref d, ref X[12], MD5_S41, MD5_T53);
        II (ref d, ref a, ref b, ref c, ref X[ 3], MD5_S42, MD5_T54);
        II (ref c, ref d, ref a, ref b, ref X[10], MD5_S43, MD5_T55);
        II (ref b, ref c, ref d, ref a, ref X[ 1], MD5_S44, MD5_T56);
        II (ref a, ref b, ref c, ref d, ref X[ 8], MD5_S41, MD5_T57);
        II (ref d, ref a, ref b, ref c, ref X[15], MD5_S42, MD5_T58);
        II (ref c, ref d, ref a, ref b, ref X[ 6], MD5_S43, MD5_T59);
        II (ref b, ref c, ref d, ref a, ref X[13], MD5_S44, MD5_T60);
        II (ref a, ref b, ref c, ref d, ref X[ 4], MD5_S41, MD5_T61);
        II (ref d, ref a, ref b, ref c, ref X[11], MD5_S42, MD5_T62);
        II (ref c, ref d, ref a, ref b, ref X[ 2], MD5_S43, MD5_T63);
        II (ref b, ref c, ref d, ref a, ref X[ 9], MD5_S44, MD5_T64);

        //add the transformed values to the current checksum
        //Increment each register by the value it had before transformation started
        m_lMD5[0] += a;
        m_lMD5[1] += b;
        m_lMD5[2] += c;
        m_lMD5[3] += d;
    }

        //Do a circular left shift by n number of bits
        public DWORD RotateLeft(DWORD x, uint n)
        {
         //check that DWORD is 4 bytes long - true in Visual C++ 6 and 32 bit Windows
         // sizeof(x);

         //rotate and return x
         return (x << (int)n) | (x >> (int)(32-n));
        }

        public void DWordToByte(byte[] Output,DWORD[] Input, int nLength )
        {
        
         //transfer the data by shifting and copying
         int input = 0;
         int output = 0;
         for ( ; output < nLength; input++, output += 4)
         {
          Output[output] =   (byte)(Input[input] & 0xff);  //extract the lowest 8 bits (lowest 1 byte), truncate Input to a 1-byte value and assign to first cell of Output
          Output[output+1] = (byte)((Input[input] >> 8) & 0xff); //right-shift Input by 8 bits, extract the lowest 8 bits,truncate Input to a 1-byte value and assign to second cell of Output
          Output[output+2] = (byte)((Input[input] >> 16) & 0xff);
          Output[output+3] = (byte)((Input[input] >> 24) & 0xff);
         }
}

        public void ByteToDWord(DWORD[] Output,byte[] Input, uint nLength)
        {
        
         //initialisations
         uint output=0; //index to Output array
         uint input=0; //index to Input array

         //transfer the data by shifting and copying
         for ( ; input < nLength; output++, input += 4)
         {
          Output[output] = (DWORD)Input[input]   | // 0x 00      00     00    cell0 |
             (DWORD)Input[input+1] << 8 |       // 0x 00      00   cell1    00   |
             (DWORD)Input[input+2] << 16 |      // 0x 00     cell2   00     00   |
             (DWORD)Input[input+3] << 24;       // 0x cell3   00     00     00
         }
        }

        //Function FF(a, b, c, d, Mj, s, ti): a = b + ((a + F(b, c, d) + X[k] + Ti) <<< s) 
        // F(X,Y,Z) = XY v (not(X) & Z) 
        public void FF(ref DWORD A, ref DWORD B,ref DWORD C, ref DWORD D,ref DWORD X,DWORD S, DWORD T)
        {
         DWORD F = (B & C) | (~B & D);
         A += F + X + T;
         A = RotateLeft(A, S);
         A += B;
        }


        //Function GG(a, b, c, d, Mj, s, ti): a = b + ((a + G(b,c,d) + X[k] + T[i]) <<< s)
        //G(X,Y,Z) = XZ v (Y& not(Z))
        public void GG(ref DWORD A,ref DWORD B,ref DWORD C,ref DWORD D,ref DWORD X,DWORD S,DWORD T)
        {
         DWORD G = (B & D) | (C & ~D);
         A += G + X + T;
         A = RotateLeft(A, S);
         A += B;
        }


        //Function HH(a, b, c, d, Mj, s, ti): a = b + ((a + H(b,c,d) + X[k] + T[i]) <<< s)
        //H(X,Y,Z) = X xor Y xor Z
        public void HH( ref DWORD A,ref DWORD B,ref DWORD C,ref DWORD D,ref DWORD X, DWORD S, DWORD T)
        {
         DWORD H = (B ^ C ^ D);
         A += H + X + T;
         A = RotateLeft(A, S);
         A += B;
        }


        //Function II(a, b, c, d, Mj, s, ti): a = b + ((a + I(b,c,d) + X[k] + T[i]) <<< s)
        //I(X,Y,Z) = Y xor (X v not(Z))
        public void II( ref DWORD A,ref DWORD B,ref DWORD C,ref DWORD D,ref DWORD X, DWORD S, DWORD T)
        {
         DWORD I = (C ^ (B | ~D));
         A += I + X + T;
         A = RotateLeft(A, S);
         A += B;
        }
    }
}


