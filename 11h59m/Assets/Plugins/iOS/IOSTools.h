//
//  IOSTools.h
//  UnityPluginsIOS
//
//  Created by Bearjuny on 2014. 9. 15..
//  Copyright (c) 2014년 Joonyoung Ko. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface IOSTools : NSObject

+ (NSString*) CharToNSString:(char *)value;
+ (const char *) NSStringToChar:(NSString *)value;
+ (char *) cStringCopy:(const char *)value;

@end
