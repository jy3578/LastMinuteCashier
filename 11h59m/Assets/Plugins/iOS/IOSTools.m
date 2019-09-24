//
//  IOSTools.m
//  UnityPluginsIOS
//
//  Created by Bearjuny on 2014. 9. 15..
//  Copyright (c) 2014년 Joonyoung Ko. All rights reserved.
//

#import "IOSTools.h"

@implementation IOSTools

+(NSString *) CharToNSString:(char *)value
{
    if (value != NULL) {
        return [NSString stringWithUTF8String: value];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}

+ (const char *)NSStringToChar:(NSString *)value
{
    return [value UTF8String];
}

+ (char *) cStringCopy:(const char*)value
{
    if (value == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(value) + 1);
    strcpy(res, value);
    
    return res;
}

@end
