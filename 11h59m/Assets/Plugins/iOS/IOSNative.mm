//
//  IOSNative.m
//  UnityPluginsIOS
//
//  Created by Bearjuny on 2014. 9. 15..
//  Copyright (c) 2014년 Joonyoung Ko. All rights reserved.
//

#import "IOSNative.h"
#import "IOSTools.h"
#import "RateDialogDelegate.h"

@implementation IOSNative

static UIAlertView* _currentAllert = nil;

+ (void) UnregisterAllertView {
    if(_currentAllert != nil)
    {
        [_currentAllert release];
        _currentAllert = nil;
    }
}

+(void) ShowRateDialog: (NSString *)title message:(NSString*)msg yes:(NSString*)yes later:(NSString*)later no:(NSString*)no
{
    UIAlertView *alert = [[UIAlertView alloc] init];
    
    [alert setTitle:title];
    [alert setMessage:msg];
    [alert setDelegate: [[RateDialogDelegate alloc] init]];
    
    [alert addButtonWithTitle:yes];
    [alert addButtonWithTitle:later];
    [alert addButtonWithTitle:no];
    
    [alert show];
    
    _currentAllert = alert;
}

+(NSString *) GetPackageName
{
    return [[NSBundle mainBundle] bundleIdentifier];
}

+(NSString *) GetAppVersion
{
    return [[NSBundle mainBundle] objectForInfoDictionaryKey: @"CFBundleShortVersionString"];
    
}

+(NSString *) GetBundleVersion
{
    return [[NSBundle mainBundle] objectForInfoDictionaryKey: (NSString *)kCFBundleVersionKey];
}

extern "C"
{
    void _ShowRateDialog(char* title, char* message, char* yes, char* later, char* no)
    {
        [IOSNative ShowRateDialog:[IOSTools CharToNSString:title] message:[IOSTools CharToNSString:message] yes:[IOSTools CharToNSString:yes] later:[IOSTools CharToNSString:later] no:[IOSTools CharToNSString:no]];
    }
    
   char *_GetPackageName()
    {
        return [IOSTools cStringCopy:[IOSTools NSStringToChar:[IOSNative GetPackageName]]];
    }
    
   char *_GetAppVersion()
    {
        return [IOSTools cStringCopy:[IOSTools NSStringToChar:[IOSNative GetAppVersion]]];
    }
    
   char *_GetBundleVersion()
    {
        return [IOSTools cStringCopy:[IOSTools NSStringToChar:[IOSNative GetBundleVersion]]];
    }
}

@end
